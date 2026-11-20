import { Injectable } from '@angular/core';
import { IAuthStateService, IPlayer } from '../models/game.interfaces';

@Injectable({
  providedIn: 'root'
})
export class AuthStateService implements IAuthStateService {
  private loggedInPlayer: IPlayer | null = null;
  private loggedInAdmin: IPlayer | null = null;

  getPlayer(): IPlayer | null {
    return this.loggedInPlayer;
  }

  setPlayer(player: IPlayer | null): void {
    this.loggedInPlayer = player;
  }

  getAdmin(): IPlayer | null {
    return this.loggedInAdmin;
  }

  setAdmin(admin: IPlayer | null): void {
    this.loggedInAdmin = admin;
  }

  logoutPlayer(): void {
    this.loggedInPlayer = null;
  }

  logoutAdmin(): void {
    this.loggedInAdmin = null;
  }
}

