import { Routes } from '@angular/router';
import { GameComponent } from './components/game/game.component';
import { PlayerComponent } from './components/player/player.component';
import { AdminComponent } from './components/admin/admin.component';

export const routes: Routes = [
  { path: '', redirectTo: 'game', pathMatch: 'full' },
  { path: 'game', component: GameComponent },
  { path: 'player', component: PlayerComponent },
  { path: 'admin', component: AdminComponent },
  { path: '**', redirectTo: 'game' }
];

