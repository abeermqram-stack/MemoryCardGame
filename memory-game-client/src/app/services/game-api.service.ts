import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  ICardItem,
  IGameApiClient,
  IGameSession,
  IPlayer
} from '../models/game.interfaces';

@Injectable({
  providedIn: 'root'
})
export class GameApiService implements IGameApiClient {
  private apiUrl = 'http://localhost:5150/api';

  constructor(private http: HttpClient) {}

  getCardsByLevel(level: number): Observable<ICardItem[]> {
    return this.http.get<ICardItem[]>(`${this.apiUrl}/Cards/level/${level}`);
  }

  getAllCards(): Observable<ICardItem[]> {
    return this.http.get<ICardItem[]>(`${this.apiUrl}/Cards`);
  }

  searchCards(keyword: string): Observable<ICardItem[]> {
    const params = new HttpParams().set('keyword', keyword);
    return this.http.get<ICardItem[]>(`${this.apiUrl}/Cards/search`, { params });
  }

  addCard(card: Omit<ICardItem, 'cardId'>): Observable<ICardItem> {
    return this.http.post<ICardItem>(`${this.apiUrl}/Cards`, card);
  }

  updateCard(id: number, card: ICardItem): Observable<any> {
    return this.http.put(`${this.apiUrl}/Cards/${id}`, card);
  }

  updateCardTitle(id: number, title: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/Cards/${id}/update-title`, { title });
  }

  deleteCard(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Cards/${id}`);
  }

  getAllSessions(): Observable<IGameSession[]> {
    return this.http.get<IGameSession[]>(`${this.apiUrl}/GameSessions`);
  }

  addSession(session: IGameSession): Observable<IGameSession> {
    return this.http.post<IGameSession>(`${this.apiUrl}/GameSessions`, session);
  }

  deleteSession(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/GameSessions/${id}`);
  }

  getAllPlayers(): Observable<IPlayer[]> {
    return this.http.get<IPlayer[]>(`${this.apiUrl}/Players`);
  }

  getPlayerById(id: number): Observable<IPlayer> {
    return this.http.get<IPlayer>(`${this.apiUrl}/Players/${id}`);
  }

  login(email: string, password: string): Observable<IPlayer> {
    const params = new HttpParams()
      .set('email', email)
      .set('password', password);
    return this.http.get<IPlayer>(`${this.apiUrl}/Players/login`, { params });
  }

  registerPlayer(player: Partial<IPlayer>): Observable<IPlayer> {
    return this.http.post<IPlayer>(`${this.apiUrl}/Players`, player);
  }

  deletePlayer(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Players/${id}`);
  }
}
