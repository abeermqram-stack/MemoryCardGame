import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ICardItem, IGameSession, IPlayer } from '../../models/game.interfaces';
import { GameApiService } from '../../services/game-api.service';
import { AuthStateService } from '../../services/auth-state.service';
import { TimeFormatPipe } from '../../pipes/time-format.pipe';

export type FeedbackType = 'success' | 'error' | 'info';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, FormsModule, TimeFormatPipe],
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit, OnDestroy {
  adminEmail: string = '';
  adminPassword: string = '';
  adminLoginError: string = '';

  feedbackMessage: string = '';
  feedbackType: FeedbackType = 'info';
  private feedbackTimer: any = null;

  allCards: ICardItem[] = [];
  displayedCards: ICardItem[] = [];
  players: IPlayer[] = [];
  sessions: IGameSession[] = [];
  searchKeyword: string = '';
  selectedAdminLevel: number = 1;

  isEditingCard: boolean = false;
  cardForm: ICardItem = {
    cardId: 0,
    title: '',
    icon: '🌟',
    category: 'عام',
    accentColor: '#2c6e7f',
    levelDifficulty: 1,
    points: 100,
    isActive: true
  };

  constructor(
    private apiService: GameApiService,
    private authState: AuthStateService
  ) {}

  get loggedInAdmin(): IPlayer | null {
    return this.authState.getAdmin();
  }

  private isCardInLevel(card: ICardItem, level: number): boolean {
    if (level === 1) {
      return card.levelDifficulty === 1;
    }
    return card.levelDifficulty === 2 || (card.levelDifficulty === 1 && card.cardId <= 6);
  }

  get level1Count(): number {
    return this.allCards.filter(c => this.isCardInLevel(c, 1)).length;
  }

  get level2Count(): number {
    return this.allCards.filter(c => this.isCardInLevel(c, 2)).length;
  }

  get visibleCards(): ICardItem[] {
    return this.displayedCards.filter(c => this.isCardInLevel(c, this.selectedAdminLevel));
  }

  ngOnInit(): void {
    if (this.loggedInAdmin) {
      this.loadData();
    }
  }

  ngOnDestroy(): void {
    if (this.feedbackTimer) {
      clearTimeout(this.feedbackTimer);
      this.feedbackTimer = null;
    }
  }

  showFeedback(message: string, type: FeedbackType = 'success'): void {
    this.feedbackMessage = message;
    this.feedbackType = type;
    if (this.feedbackTimer) {
      clearTimeout(this.feedbackTimer);
    }
    this.feedbackTimer = setTimeout(() => {
      this.feedbackMessage = '';
      this.feedbackTimer = null;
    }, 5000);
  }

  dismissFeedback(): void {
    this.feedbackMessage = '';
    if (this.feedbackTimer) {
      clearTimeout(this.feedbackTimer);
      this.feedbackTimer = null;
    }
  }

  loadData(): void {
    this.apiService.getAllCards().subscribe(cards => {
      this.allCards = cards;
      this.displayedCards = cards;
    });

    this.apiService.getAllPlayers().subscribe(players => {
      this.players = players.filter(p => p.role !== 'Admin');
    });

    this.apiService.getAllSessions().subscribe(sessions => {
      this.sessions = sessions;
    });
  }

  selectAdminLevel(level: number): void {
    this.selectedAdminLevel = level;
    this.searchKeyword = '';
    this.displayedCards = this.allCards;
    if (!this.isEditingCard) {
      this.cardForm.levelDifficulty = level;
    }
  }

  onFormLevelChange(level: number): void {
    this.cardForm.levelDifficulty = Number(level);
    if (!this.isEditingCard) {
      this.selectedAdminLevel = Number(level);
      this.searchKeyword = '';
      this.displayedCards = this.allCards;
    }
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
          this.loadData();
          this.showFeedback(`مرحباً بكِ ${user.username}! تم تسجيل الدخول كمشرف بنجاح.`, 'success');
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
    this.allCards = [];
    this.displayedCards = [];
    this.players = [];
    this.sessions = [];
    this.selectedAdminLevel = 1;
    this.resetForm();
    this.showFeedback('تم تسجيل خروج المشرف بنجاح.', 'info');
  }

  searchCards(): void {
    const trimmed = this.searchKeyword.trim();
    if (!trimmed) {
      this.displayedCards = this.allCards;
      this.showFeedback(`تم عرض جميع بطاقات المستوى ${this.selectedAdminLevel === 1 ? 'الأول' : 'الثاني'}.`, 'info');
      return;
    }

    this.apiService.searchCards(trimmed).subscribe({
      next: cards => {
        this.displayedCards = cards;
        const levelMatches = cards.filter(c => this.isCardInLevel(c, this.selectedAdminLevel)).length;
        const levelLabel = this.selectedAdminLevel === 1 ? 'الأول' : 'الثاني';
        if (levelMatches > 0) {
          this.showFeedback(`تم العثور على ${levelMatches} بطاقة في المستوى ${levelLabel} مطابقة للبحث "${trimmed}".`, 'info');
        } else {
          this.showFeedback(`لا توجد بطاقات في المستوى ${levelLabel} مطابقة للبحث "${trimmed}".`, 'info');
        }
      },
      error: () => {
        this.showFeedback('تعذر تنفيذ البحث عن البطاقات.', 'error');
      }
    });
  }

  clearSearch(): void {
    this.searchKeyword = '';
    this.displayedCards = this.allCards;
    this.showFeedback(`تم إلغاء التصفية وإظهار جميع بطاقات المستوى ${this.selectedAdminLevel === 1 ? 'الأول' : 'الثاني'}.`, 'info');
  }

  saveCard(): void {
    if (!this.loggedInAdmin) return;
    const title = this.cardForm.title.trim();
    const icon = this.cardForm.icon.trim();
    const category = this.cardForm.category.trim() || 'عام';
    const points = Number(this.cardForm.points);

    if (!title || !icon) {
      this.showFeedback('الرجاء إدخال اسم البطاقة والرمز.', 'error');
      return;
    }

    if (isNaN(points) || points < 10 || points > 1000) {
      this.showFeedback('يجب أن تكون نقاط البطاقة بين 10 و 1000 نقطة.', 'error');
      return;
    }

    const payload: ICardItem = {
      ...this.cardForm,
      title,
      icon,
      category,
      points
    };

    if (this.isEditingCard) {
      this.apiService.updateCard(payload.cardId, payload).subscribe({
        next: () => {
          this.selectedAdminLevel = payload.levelDifficulty;
          this.searchKeyword = '';
          this.resetForm();
          this.loadData();
          this.showFeedback(`تم تعديل البطاقة "${title}" (${points} نقطة) بنجاح وتحديثها في اللعب.`, 'success');
        },
        error: () => {
          this.showFeedback('تعذر حفظ تعديلات البطاقة، يرجى المحاولة مرة أخرى.', 'error');
        }
      });
    } else {
      const { cardId, ...newCard } = payload;
      this.apiService.addCard(newCard).subscribe({
        next: created => {
          const savedPoints = created?.points ?? points;
          this.selectedAdminLevel = payload.levelDifficulty;
          this.searchKeyword = '';
          this.resetForm();
          this.loadData();
          const levelLabel = payload.levelDifficulty === 1 ? 'الأول' : 'الثاني';
          this.showFeedback(`تمت إضافة البطاقة "${title}" بقيمة ${savedPoints} نقطة إلى المستوى ${levelLabel} وأصبحت متاحة في اللعب.`, 'success');
        },
        error: () => {
          this.showFeedback('تعذر إضافة البطاقة، يرجى التحقق من البيانات.', 'error');
        }
      });
    }
  }

  editCard(card: ICardItem): void {
    if (!this.loggedInAdmin) return;
    this.isEditingCard = true;
    this.cardForm = {
      ...card,
      points: card.points && card.points >= 10 ? card.points : 100
    };
    this.showFeedback(`جاري تعديل البطاقة "${card.title}" — عدّلي البيانات ثم اضغطي "حفظ التعديل".`, 'info');
  }

  cancelEdit(): void {
    this.resetForm();
    this.showFeedback('تم إلغاء تعديل البطاقة.', 'info');
  }

  deleteCard(card: ICardItem): void {
    if (!this.loggedInAdmin) return;
    const confirmed = window.confirm(`هل أنتِ متأكدة من حذف البطاقة "${card.title}"؟`);
    if (!confirmed) return;

    this.apiService.deleteCard(card.cardId).subscribe({
      next: () => {
        if (this.isEditingCard && this.cardForm.cardId === card.cardId) {
          this.resetForm();
        }
        this.loadData();
        this.showFeedback(`تم حذف البطاقة "${card.title}" بنجاح.`, 'success');
      },
      error: () => {
        this.showFeedback('تعذر حذف البطاقة، يرجى المحاولة مرة أخرى.', 'error');
      }
    });
  }

  deleteSession(session: IGameSession): void {
    if (!this.loggedInAdmin || !session.sessionId) return;
    const playerName = session.player?.username || 'اللاعب';
    const confirmed = window.confirm(
      `هل أنتِ متأكدة من حذف نتيجة "${playerName}" (${session.score} نقطة)؟`
    );
    if (!confirmed) return;

    this.apiService.deleteSession(session.sessionId).subscribe({
      next: () => {
        this.loadData();
        this.showFeedback(`تم حذف نتيجة "${playerName}" (${session.score} نقطة) وتحديث الإحصائيات بنجاح.`, 'success');
      },
      error: () => {
        this.showFeedback('تعذر حذف النتيجة، يرجى المحاولة مرة أخرى.', 'error');
      }
    });
  }

  deletePlayer(player: IPlayer): void {
    if (!this.loggedInAdmin) return;
    const confirmed = window.confirm(
      `هل أنتِ متأكدة من حذف حساب "${player.username}" (${player.email}) نهائياً؟ سيتم حذف الحساب وجميع نتائجه وبياناته السابقة من قاعدة البيانات.`
    );
    if (!confirmed) return;

    this.apiService.deletePlayer(player.playerId).subscribe({
      next: () => {
        if (this.authState.getPlayer()?.playerId === player.playerId) {
          this.authState.logoutPlayer();
        }
        this.loadData();
        this.showFeedback(`تم حذف حساب "${player.username}" وجميع بياناته نهائياً من قاعدة البيانات.`, 'success');
      },
      error: err => {
        this.showFeedback(err?.error?.message || 'تعذر حذف حساب اللاعب.', 'error');
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
      levelDifficulty: this.selectedAdminLevel,
      points: 100,
      isActive: true
    };
  }
}

