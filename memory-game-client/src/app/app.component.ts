import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  IBoardCard,
  ICardItem,
  IGameSession,
  IPlayer
} from './models/game.interfaces';
import { GameApiService } from './services/game-api.service';
import { MemoryGameService } from './services/memory-game.service';
import { TimeFormatPipe } from './pipes/time-format.pipe';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, TimeFormatPipe],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  // الصفحات: game (اللعب والنتائج) | playerAuth (دخول/تسجيل لاعبة) | admin (خاص بالمشرف فقط)
  currentView: 'game' | 'playerAuth' | 'admin' = 'game';

  // المستوى الحالي: 1 (6 أزواج) أو 2 (10 أزواج)
  selectedLevel: number = 1;

  // مصفوفة البطاقات في اللعبة
  boardCards: IBoardCard[] = [];
  flippedCards: IBoardCard[] = [];
  isBoardLocked: boolean = false;

  // عدادات اللعبة: المحاولات، الوقت، النقاط
  attempts: number = 0;
  matchedPairsCount: number = 0;
  totalPairsCount: number = 6;
  elapsedSeconds: number = 0;
  currentScore: number = 1000;
  isGameStarted: boolean = false;
  isGameWon: boolean = false;
  private timerInterval: any = null;

  // البيانات
  allCards: ICardItem[] = [];
  sessions: IGameSession[] = [];

  // 1. حساب اللاعبة الحالية (Player)
  loggedInPlayer: IPlayer | null = null;
  playerAuthMode: 'login' | 'register' = 'login';
  playerLoginEmail: string = '';
  playerLoginPassword: string = '';
  playerRegName: string = '';
  playerRegEmail: string = '';
  playerRegPassword: string = '';
  playerAuthMessage: string = '';

  // 2. حساب المشرف (Admin) - منفصل ومستقل بصلاحيات الحذف والتعديل والإضافة
  loggedInAdmin: IPlayer | null = null;
  adminEmail: string = 'abeer@gmail.com';
  adminPassword: string = '1234';
  adminLoginError: string = '';

  // نموذج إضافة وتعديل بطاقة (للأدمن فقط)
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
    private gameEngine: MemoryGameService
  ) {}

  ngOnInit(): void {
    this.startNewGame(1);
    this.loadData();
  }

  ngOnDestroy(): void {
    this.stopTimer();
  }

  loadData(): void {
    this.apiService.getAllCards().subscribe(cards => {
      this.allCards = cards;
    });

    this.apiService.getAllSessions().subscribe(sessions => {
      this.sessions = sessions;
    });
  }

  // ==========================================================================
  // 1. طريقة اللعب (6 أزواج للمستوى الأول، 10 أزواج للمستوى الثاني)
  // ==========================================================================

  startNewGame(level: number): void {
    this.selectedLevel = level;
    this.totalPairsCount = level === 1 ? 6 : 10;
    this.attempts = 0;
    this.matchedPairsCount = 0;
    this.elapsedSeconds = 0;
    this.currentScore = level === 1 ? 1000 : 1800;
    this.flippedCards = [];
    this.isBoardLocked = false;
    this.isGameWon = false;
    this.isGameStarted = false;
    this.stopTimer();

    this.apiService.getCardsByLevel(level).subscribe(cards => {
      this.boardCards = this.gameEngine.buildBoardDeck(cards, level);
    });
  }

  onCardClick(card: IBoardCard): void {
    if (this.isBoardLocked || card.isFlipped || card.isMatched || this.isGameWon) {
      return;
    }

    if (!this.isGameStarted) {
      this.isGameStarted = true;
      this.startTimer();
    }

    card.isFlipped = true;
    this.flippedCards.push(card);

    if (this.flippedCards.length === 2) {
      this.attempts++;
      this.currentScore = this.gameEngine.calculateScore(
        this.selectedLevel,
        this.attempts,
        this.elapsedSeconds
      );
      this.checkMatch();
    }
  }

  checkMatch(): void {
    const first = this.flippedCards[0];
    const second = this.flippedCards[1];

    if (this.gameEngine.areCardsMatching(first, second)) {
      first.isMatched = true;
      second.isMatched = true;
      this.flippedCards = [];
      this.matchedPairsCount++;

      if (this.matchedPairsCount === this.totalPairsCount) {
        this.stopTimer();
        this.isGameWon = true;
        this.saveWinSession();
      }
    } else {
      this.isBoardLocked = true;
      setTimeout(() => {
        first.isFlipped = false;
        second.isFlipped = false;
        this.flippedCards = [];
        this.isBoardLocked = false;
      }, 750);
    }
  }

  saveWinSession(): void {
    const newSession: IGameSession = {
      level: this.selectedLevel,
      pairsCount: this.totalPairsCount,
      attempts: this.attempts,
      timeElapsedSeconds: Math.max(1, this.elapsedSeconds),
      score: this.currentScore,
      isCompleted: true,
      playerId: this.loggedInPlayer ? this.loggedInPlayer.playerId : 2
    };

    this.apiService.addSession(newSession).subscribe(() => {
      this.loadData();
    });
  }

  startTimer(): void {
    this.stopTimer();
    this.timerInterval = setInterval(() => {
      this.elapsedSeconds++;
      this.currentScore = this.gameEngine.calculateScore(
        this.selectedLevel,
        Math.max(1, this.attempts),
        this.elapsedSeconds
      );
    }, 1000);
  }

  stopTimer(): void {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
      this.timerInterval = null;
    }
  }

  // ==========================================================================
  // 2. تسجيل دخول اللاعبة أو إنشاء حساب جديد (بدون صلاحيات أدمن)
  // ==========================================================================

  loginAsPlayer(): void {
    this.playerAuthMessage = '';
    if (!this.playerLoginEmail.trim() || !this.playerLoginPassword.trim()) {
      this.playerAuthMessage = 'الرجاء إدخال البريد الإلكتروني وكلمة المرور';
      return;
    }

    this.apiService.login(this.playerLoginEmail, this.playerLoginPassword).subscribe({
      next: player => {
        this.loggedInPlayer = player;
        this.loadData();
      },
      error: () => {
        this.playerAuthMessage = 'البريد الإلكتروني أو كلمة المرور غير صحيحة';
      }
    });
  }

  registerNewPlayer(): void {
    this.playerAuthMessage = '';
    if (!this.playerRegName.trim() || !this.playerRegEmail.trim() || !this.playerRegPassword.trim()) {
      this.playerAuthMessage = 'الرجاء تعبئة الاسم والبريد الإلكتروني وكلمة المرور';
      return;
    }

    const newPlayer: Partial<IPlayer> = {
      username: this.playerRegName,
      email: this.playerRegEmail,
      password: this.playerRegPassword,
      role: 'Player' // تُسجل دائماً كلاعبة عادية وليس أدمن
    };

    this.apiService.registerPlayer(newPlayer).subscribe({
      next: created => {
        this.loggedInPlayer = created;
        this.playerRegName = '';
        this.playerRegEmail = '';
        this.playerRegPassword = '';
        this.loadData();
      },
      error: () => {
        this.playerAuthMessage = 'تعذر إنشاء الحساب، تأكدي من البيانات';
      }
    });
  }

  logoutPlayer(): void {
    this.loggedInPlayer = null;
  }

  // ==========================================================================
  // 3. دخول المشرف (Admin) وعمليات الحذف والتعديل والإضافة
  // ==========================================================================

  loginAsAdmin(): void {
    this.adminLoginError = '';
    this.apiService.login(this.adminEmail, this.adminPassword).subscribe({
      next: user => {
        if (user.role === 'Admin') {
          this.loggedInAdmin = user;
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
    this.loggedInAdmin = null;
  }

  saveCard(): void {
    if (!this.loggedInAdmin) return;
    if (!this.cardForm.title.trim() || !this.cardForm.icon.trim()) return;

    if (this.isEditingCard) {
      this.apiService.updateCard(this.cardForm.cardId, this.cardForm).subscribe(() => {
        this.resetForm();
        this.loadData();
        this.startNewGame(this.selectedLevel);
      });
    } else {
      const { cardId, ...newCard } = this.cardForm;
      this.apiService.addCard(newCard).subscribe(() => {
        this.resetForm();
        this.loadData();
        this.startNewGame(this.selectedLevel);
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
      this.startNewGame(this.selectedLevel);
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
