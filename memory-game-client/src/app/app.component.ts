import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { IPlayer } from './models/game.interfaces';
import { AuthStateService } from './services/auth-state.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  appTitle: string = 'لعبة بطاقات الذاكرة';

  constructor(private authState: AuthStateService) {}

  get loggedInPlayer(): IPlayer | null {
    return this.authState.getPlayer();
  }
}
