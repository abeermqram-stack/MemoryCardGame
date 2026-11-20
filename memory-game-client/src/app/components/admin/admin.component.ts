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
  adminEmail: string = 'abeer@gmail.com';
  adminPassword: string = '1234';
  adminLoginError: string = '';

  allCards: ICardItem[] = [];
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
    this.loadData();
  }

  loadData(): void {
    this.apiService.getAllCards().subscribe(cards => {
      this.allCards = cards;
    });

    this.apiService.getAllSessions().subscribe(sessions => {
      this.sessions = sessions;
    });
  }

  loginAsAdmin(): void {
    this.adminLoginError = '';
    this.apiService.login(this.adminEmail, this.adminPassword).subscribe({
      next: user => {
        if (user.role === 'Admin') {
          this.authState.setAdmin(user);
          this.loadData();
        } else {
          this.adminLoginError = 'عفواً، هذا الحساب للاعبة عادية وليس حساب مشرف (Admin)';
        }
      },
      error: () => {
        this.adminLoginError = 'بيانات المشرف غير صحيحة';
      }
    });
  }

  logoutAdmin(): void {
    this.authState.logoutAdmin();
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

