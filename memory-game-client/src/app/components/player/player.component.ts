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
      this.apiService.getPlayerById(current.playerId).subscribe(fresh => {
        this.authState.setPlayer(fresh);
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
    if (!this.playerLoginEmail.trim() || !this.playerLoginPassword.trim()) {
      this.playerAuthMessage = 'الرجاء إدخال البريد الإلكتروني وكلمة المرور';
      return;
    }

    this.apiService.login(this.playerLoginEmail, this.playerLoginPassword).subscribe({
      next: player => {
        this.authState.setPlayer(player);
        this.loadSessions();
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
      error: () => {
        this.playerAuthMessage = 'تعذر إنشاء الحساب، تأكدي من البيانات';
      }
    });
  }

  logoutPlayer(): void {
    this.authState.logoutPlayer();
  }
}

