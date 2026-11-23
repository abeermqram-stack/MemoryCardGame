import { Injectable } from '@angular/core';
import { IAuthStateService, IPlayer } from '../models/game.interfaces';

@Injectable({
  providedIn: 'root'
})
export class AuthStateService implements IAuthStateService {
  private loggedInPlayer: IPlayer | null = null;
  private playerToken: string | null = null;
  private loggedInAdmin: IPlayer | null = null;
  private adminToken: string | null = null;

  getPlayer(): IPlayer | null {
    return this.loggedInPlayer;
  }

  getPlayerToken(): string | null {
    return this.playerToken;
  }

  setPlayer(player: IPlayer | null, token?: string | null): void {
    this.loggedInPlayer = player;
    if (token !== undefined) {
      this.playerToken = token;
    } else if (!player) {
      this.playerToken = null;
    }
  }

  getAdmin(): IPlayer | null {
    return this.loggedInAdmin;
  }

  getAdminToken(): string | null {
    return this.adminToken;
  }

  setAdmin(admin: IPlayer | null, token?: string | null): void {
    this.loggedInAdmin = admin;
    if (token !== undefined) {
      this.adminToken = token;
    } else if (!admin) {
      this.adminToken = null;
    }
  }

  logoutPlayer(): void {
    this.loggedInPlayer = null;
    this.playerToken = null;
  }

  logoutAdmin(): void {
    this.loggedInAdmin = null;
    this.adminToken = null;
  }
}
