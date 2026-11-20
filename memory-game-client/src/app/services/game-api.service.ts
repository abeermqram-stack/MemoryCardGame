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

  // جلب البطاقات حسب المستوى (1 أو 2)
  getCardsByLevel(level: number): Observable<ICardItem[]> {
    return this.http.get<ICardItem[]>(`${this.apiUrl}/Cards/level/${level}`);
  }

  // جلب جميع البطاقات
  getAllCards(): Observable<ICardItem[]> {
    return this.http.get<ICardItem[]>(`${this.apiUrl}/Cards`);
  }

  // البحث عن البطاقات بالكلمة المفتاحية ([FromQuery])
  searchCards(keyword: string): Observable<ICardItem[]> {
    const params = new HttpParams().set('keyword', keyword);
    return this.http.get<ICardItem[]>(`${this.apiUrl}/Cards/search`, { params });
  }

  // إضافة بطاقة جديدة (Add)
  addCard(card: Omit<ICardItem, 'cardId'>): Observable<ICardItem> {
    return this.http.post<ICardItem>(`${this.apiUrl}/Cards`, card);
  }

  // تحديث بطاقة بالكامل (Update)
  updateCard(id: number, card: ICardItem): Observable<any> {
    return this.http.put(`${this.apiUrl}/Cards/${id}`, card);
  }

  // تحديث عنوان البطاقة فقط (Specialized Update DTO)
  updateCardTitle(id: number, title: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/Cards/${id}/update-title`, { title });
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

  // جلب بيانات لاعبة مع ملفها الشخصي (1:1) وجلساتها (1:N) وإنجازاتها (N:M)
  getPlayerById(id: number): Observable<IPlayer> {
    return this.http.get<IPlayer>(`${this.apiUrl}/Players/${id}`);
  }

  // تسجيل الدخول ([FromQuery])
  login(email: string, password: string): Observable<IPlayer> {
    const params = new HttpParams()
      .set('email', email)
      .set('password', password);
    return this.http.get<IPlayer>(`${this.apiUrl}/Players/login`, { params });
  }

  // إنشاء حساب لاعبة جديدة (Add Player)
  registerPlayer(player: Partial<IPlayer>): Observable<IPlayer> {
    return this.http.post<IPlayer>(`${this.apiUrl}/Players`, player);
  }
}
