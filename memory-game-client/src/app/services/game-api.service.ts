import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
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

  // جلب البطاقات حسب المستوى (1 أو 2)
  getCardsByLevel(level: number): Observable<ICardItem[]> {
    return this.http.get<ICardItem[]>(`${this.apiUrl}/Cards/level/${level}`);
  }

  // جلب جميع البطاقات
  getAllCards(): Observable<ICardItem[]> {
    return this.http.get<ICardItem[]>(`${this.apiUrl}/Cards`);
  }

  // إضافة بطاقة جديدة (Add)
  addCard(card: Omit<ICardItem, 'cardId'>): Observable<ICardItem> {
    return this.http.post<ICardItem>(`${this.apiUrl}/Cards`, card);
  }

  // تحديث بطاقة (Update)
  updateCard(id: number, card: ICardItem): Observable<any> {
    return this.http.put(`${this.apiUrl}/Cards/${id}`, card);
  }

  // حذف بطاقة (Delete)
  deleteCard(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Cards/${id}`);
  }

  // جلب سجل النتائج
  getAllSessions(): Observable<IGameSession[]> {
    return this.http.get<IGameSession[]>(`${this.apiUrl}/GameSessions`);
  }

  // إضافة نتيجة جديدة بعد الفوز (Add)
  addSession(session: IGameSession): Observable<IGameSession> {
    return this.http.post<IGameSession>(`${this.apiUrl}/GameSessions`, session);
  }

  // حذف سجل نتيجة (Delete)
  deleteSession(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/GameSessions/${id}`);
  }

  // تسجيل الدخول
  login(email: string, password: string): Observable<IPlayer> {
    return this.http.get<IPlayer>(
      `${this.apiUrl}/Players/login?email=${email}&password=${password}`
    );
  }

  // إنشاء حساب لاعبة جديدة (Add Player)
  registerPlayer(player: Partial<IPlayer>): Observable<IPlayer> {
    return this.http.post<IPlayer>(`${this.apiUrl}/Players`, player);
  }
}
