import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { IGameSession, IPlayer } from '../../models/game.interfaces';
import { GameApiService } from '../../services/game-api.service';
import { AuthStateService } from '../../services/auth-state.service';
import { TimeFormatPipe } from '../../pipes/time-format.pipe';

@Component({
  selector: 'app-player',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, TimeFormatPipe],
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.css']
})
export class PlayerComponent implements OnInit {
  playerAuthMode: 'login' | 'register' = 'login';
  playerLoginEmail: string = '';
  playerLoginPassword: string = '';
  playerRegName: string = '';
  playerRegEmail: string = '';
  playerRegPassword: string = '';
  playerAuthMessage: string = '';
  showCreateAccountHint: boolean = false;

  sessions: IGameSession[] = [];

  constructor(
    private apiService: GameApiService,
    private authState: AuthStateService
  ) {}

  get loggedInPlayer(): IPlayer | null {
    return this.authState.getPlayer();
  }

  ngOnInit(): void {
    this.loadSessions();
    const current = this.authState.getPlayer();
    if (current) {
      this.apiService.getPlayerById(current.playerId).subscribe({
        next: fresh => {
          this.authState.setPlayer(fresh);
        },
        error: err => {
          if (err?.status === 404) {
            this.authState.logoutPlayer();
            this.playerAuthMode = 'login';
            this.playerAuthMessage = 'تم حذف هذا الحساب من قاعدة البيانات. يجب عليك إنشاء حساب جديد أولاً.';
            this.showCreateAccountHint = true;
          }
        }
      });
    }
  }

  loadSessions(): void {
    this.apiService.getAllSessions().subscribe(sessions => {
      this.sessions = sessions;
    });
  }

  loginAsPlayer(): void {
    this.playerAuthMessage = '';
    this.showCreateAccountHint = false;
    if (!this.playerLoginEmail.trim() || !this.playerLoginPassword.trim()) {
      this.playerAuthMessage = 'الرجاء إدخال البريد الإلكتروني وكلمة المرور';
      return;
    }

    this.apiService.login(this.playerLoginEmail.trim(), this.playerLoginPassword).subscribe({
      next: player => {
        this.authState.setPlayer(player);
        this.playerLoginEmail = '';
        this.playerLoginPassword = '';
        this.showCreateAccountHint = false;
        this.loadSessions();
      },
      error: err => {
        if (err?.status === 0) {
          this.playerAuthMessage = 'تعذر الاتصال بالخادم (API). تأكدي من تشغيل الـ Backend على المنفذ 5150.';
        } else if (err?.error?.code === 'ACCOUNT_NOT_FOUND' || err?.status === 404 || err?.message === 'ACCOUNT_NOT_FOUND') {
          this.playerAuthMessage = 'لا يوجد حساب مسجّل بهذا البريد الإلكتروني. يجب عليك إنشاء حساب جديد أولاً.';
          this.showCreateAccountHint = true;
        } else if (err?.error?.code === 'INVALID_PASSWORD' || err?.status === 401 || err?.message === 'INVALID_PASSWORD') {
          this.playerAuthMessage = 'كلمة المرور غير صحيحة، يرجى المحاولة مرة أخرى.';
        } else {
          this.playerAuthMessage = 'البريد الإلكتروني أو كلمة المرور غير صحيحة';
        }
      }
    });
  }

  switchToRegisterWithEmail(): void {
    this.playerRegEmail = this.playerLoginEmail.trim();
    this.playerAuthMode = 'register';
    this.playerAuthMessage = '';
    this.showCreateAccountHint = false;
  }

  registerNewPlayer(): void {
    this.playerAuthMessage = '';
    this.showCreateAccountHint = false;
    if (!this.playerRegName.trim() || !this.playerRegEmail.trim() || !this.playerRegPassword.trim()) {
      this.playerAuthMessage = 'الرجاء تعبئة الاسم والبريد الإلكتروني وكلمة المرور';
      return;
    }

    if (!this.playerRegEmail.trim().includes('@')) {
      this.playerAuthMessage = 'الرجاء إدخال بريد إلكتروني صحيح (مثال: name@email.com)';
      return;
    }

    const newPlayer: Partial<IPlayer> = {
      username: this.playerRegName.trim(),
      email: this.playerRegEmail.trim(),
      password: this.playerRegPassword,
      role: 'Player'
    };

    this.apiService.registerPlayer(newPlayer).subscribe({
      next: created => {
        this.authState.setPlayer(created);
        this.playerRegName = '';
        this.playerRegEmail = '';
        this.playerRegPassword = '';
        this.loadSessions();
      },
      error: err => {
        if (err?.status === 0) {
          this.playerAuthMessage = 'تعذر الاتصال بالخادم (API). تأكدي من تشغيل الـ Backend على المنفذ 5150.';
        } else if (err?.error?.code === 'EMAIL_EXISTS' || err?.status === 409 || err?.message === 'EMAIL_EXISTS') {
          this.playerAuthMessage = 'هذا البريد الإلكتروني مسجّل مسبقاً، يمكنكِ تسجيل الدخول به أو استخدام بريد آخر';
        } else {
          this.playerAuthMessage = 'تعذر إنشاء الحساب، تأكدي من صحة البيانات والبريد الإلكتروني';
        }
      }
    });
  }

  logoutPlayer(): void {
    this.authState.logoutPlayer();
    this.playerLoginEmail = '';
    this.playerLoginPassword = '';
    this.playerAuthMessage = '';
    this.showCreateAccountHint = false;
  }
}

