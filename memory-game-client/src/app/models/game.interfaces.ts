import { Observable } from 'rxjs';

// 1. Interfaces لتعريف بنية البيانات (Data Structures)
export interface ICardItem {
  cardId: number;
  title: string;
  icon: string;
  category: string;
  accentColor: string;
  levelDifficulty: number;
  isActive: boolean;
}

export interface IBoardCard {
  uniqueBoardId: number;
  cardId: number;
  title: string;
  icon: string;
  category: string;
  accentColor: string;
  isFlipped: boolean;
  isMatched: boolean;
}

export interface IPlayer {
  playerId: number;
  username: string;
  email: string;
  password?: string;
  role: string;
}

export interface IGameSession {
  sessionId?: number;
  level: number;
  pairsCount: number;
  attempts: number;
  timeElapsedSeconds: number;
  score: number;
  isCompleted: boolean;
  playedAt?: string;
  playerId: number;
  player?: IPlayer;
}

// 2. Interfaces لتعريف تواقيع الدوال (Function Signatures)
export interface IGameEngineService {
  buildBoardDeck(cards: ICardItem[], level: number): IBoardCard[];
  areCardsMatching(firstCard: IBoardCard, secondCard: IBoardCard): boolean;
  calculateScore(level: number, attempts: number, timeElapsedSeconds: number): number;
}

export interface IGameApiClient {
  getCardsByLevel(level: number): Observable<ICardItem[]>;
  getAllCards(): Observable<ICardItem[]>;
  addCard(card: Omit<ICardItem, 'cardId'>): Observable<ICardItem>;
  updateCard(id: number, card: ICardItem): Observable<any>;
  deleteCard(id: number): Observable<any>;
  getAllSessions(): Observable<IGameSession[]>;
  addSession(session: IGameSession): Observable<IGameSession>;
  deleteSession(id: number): Observable<any>;
  login(email: string, password: string): Observable<IPlayer>;
  registerPlayer(player: Partial<IPlayer>): Observable<IPlayer>;
}
