import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ICardItem, IGameSession, IPlayer } from '../../models/game.interfaces';
import { GameApiService } from '../../services/game-api.service';
import { AuthStateService } from '../../services/auth-state.service';
import { TimeFormatPipe } from '../../pipes/time-format.pipe';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, FormsModule, TimeFormatPipe],
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  adminEmail: string = '';
  adminPassword: string = '';
  adminLoginError: string = '';
  adminActionMessage: string = '';

  allCards: ICardItem[] = [];
  players: IPlayer[] = [];
  sessions: IGameSession[] = [];
  searchKeyword: string = '';

  isEditingCard: boolean = false;
  cardForm: ICardItem = {
    cardId: 0,
    title: '',
    icon: '🌟',
    category: 'عام',
    accentColor: '#2c6e7f',
    levelDifficulty: 1,
    isActive: true
  };

  constructor(
    private apiService: GameApiService,
    private authState: AuthStateService
  ) {}

  get loggedInAdmin(): IPlayer | null {
    return this.authState.getAdmin();
  }

  ngOnInit(): void {
    if (this.loggedInAdmin) {
      this.loadData();
    }
  }

  loadData(): void {
    this.apiService.getAllCards().subscribe(cards => {
      this.allCards = cards;
    });

    this.apiService.getAllPlayers().subscribe(players => {
      this.players = players.filter(p => p.role !== 'Admin');
    });

    this.apiService.getAllSessions().subscribe(sessions => {
      this.sessions = sessions;
    });
  }

  loginAsAdmin(): void {
    this.adminLoginError = '';
    if (!this.adminEmail.trim() || !this.adminPassword.trim()) {
      this.adminLoginError = 'الرجاء إدخال البريد الإلكتروني وكلمة المرور';
      return;
    }

    this.apiService.login(this.adminEmail.trim(), this.adminPassword).subscribe({
      next: user => {
        if (user.role === 'Admin') {
          this.authState.setAdmin(user);
          this.adminEmail = '';
          this.adminPassword = '';
          this.adminActionMessage = '';
          this.loadData();
        } else {
          this.adminLoginError = 'عفواً، هذا الحساب للاعب عادي وليس حساب مشرف (Admin)';
        }
      },
      error: err => {
        if (err?.status === 0) {
          this.adminLoginError = 'تعذر الاتصال بالخادم (API). تأكدي من تشغيل الـ Backend على المنفذ 5150.';
        } else if (err?.error?.code === 'ACCOUNT_NOT_FOUND' || err?.status === 404) {
          this.adminLoginError = 'لا يوجد حساب مسجّل بهذا البريد الإلكتروني';
        } else if (err?.error?.code === 'INVALID_PASSWORD' || err?.status === 401) {
          this.adminLoginError = 'كلمة المرور غير صحيحة';
        } else {
          this.adminLoginError = 'بيانات المشرف غير صحيحة';
        }
      }
    });
  }

  logoutAdmin(): void {
    this.authState.logoutAdmin();
    this.adminEmail = '';
    this.adminPassword = '';
    this.adminLoginError = '';
    this.adminActionMessage = '';
    this.allCards = [];
    this.players = [];
    this.sessions = [];
  }

  searchCards(): void {
    const trimmed = this.searchKeyword.trim();
    if (!trimmed) {
      this.loadData();
      return;
    }

    this.apiService.searchCards(trimmed).subscribe(cards => {
      this.allCards = cards;
    });
  }

  clearSearch(): void {
    this.searchKeyword = '';
    this.loadData();
  }

  saveCard(): void {
    if (!this.loggedInAdmin) return;
    if (!this.cardForm.title.trim() || !this.cardForm.icon.trim()) return;

    if (this.isEditingCard) {
      this.apiService.updateCard(this.cardForm.cardId, this.cardForm).subscribe(() => {
        this.resetForm();
        this.loadData();
      });
    } else {
      const { cardId, ...newCard } = this.cardForm;
      this.apiService.addCard(newCard).subscribe(() => {
        this.resetForm();
        this.loadData();
      });
    }
  }

  editCard(card: ICardItem): void {
    if (!this.loggedInAdmin) return;
    this.isEditingCard = true;
    this.cardForm = { ...card };
  }

  deleteCard(id: number): void {
    if (!this.loggedInAdmin) return;
    this.apiService.deleteCard(id).subscribe(() => {
      this.loadData();
    });
  }

  deleteSession(id: number | undefined): void {
    if (!this.loggedInAdmin || !id) return;
    this.apiService.deleteSession(id).subscribe(() => {
      this.loadData();
    });
  }

  deletePlayer(player: IPlayer): void {
    if (!this.loggedInAdmin) return;
    const confirmed = window.confirm(
      `هل أنتِ متأكدة من حذف حساب "${player.username}" (${player.email}) نهائياً؟ سيتم حذف الحساب وجميع نتائجه وبياناته السابقة من قاعدة البيانات.`
    );
    if (!confirmed) return;

    this.adminActionMessage = '';
    this.apiService.deletePlayer(player.playerId).subscribe({
      next: () => {
        if (this.authState.getPlayer()?.playerId === player.playerId) {
          this.authState.logoutPlayer();
        }
        this.adminActionMessage = `تم حذف حساب "${player.username}" وجميع بياناته نهائياً من قاعدة البيانات.`;
        this.loadData();
      },
      error: err => {
        this.adminActionMessage = err?.error?.message || 'تعذر حذف حساب اللاعب.';
      }
    });
  }

  resetForm(): void {
    this.isEditingCard = false;
    this.cardForm = {
      cardId: 0,
      title: '',
      icon: '🌟',
      category: 'عام',
      accentColor: '#2c6e7f',
      levelDifficulty: 1,
      isActive: true
    };
  }
}

