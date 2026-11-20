import { Observable } from 'rxjs';

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

export interface IPlayerProfile {
  profileId: number;
  avatarEmoji: string;
  preferredTheme: string;
  highestScore: number;
  totalGamesPlayed: number;
  playerId: number;
}

export interface IAchievement {
  achievementId: number;
  title: string;
  description: string;
  badgeIcon: string;
  bonusPoints: number;
}

export interface IPlayerAchievement {
  playerId: number;
  achievementId: number;
  unlockedAt: string;
  displayOrder: number;
  achievement?: IAchievement;
}

export interface IPlayer {
  playerId: number;
  username: string;
  email: string;
  password?: string;
  role: string;
  createdAt?: string;
  profile?: IPlayerProfile;
  gameSessions?: IGameSession[];
  playerAchievements?: IPlayerAchievement[];
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

export interface IGameEngineService {
  buildBoardDeck(cards: ICardItem[], level: number): IBoardCard[];
  areCardsMatching(firstCard: IBoardCard, secondCard: IBoardCard): boolean;
  calculateScore(level: number, attempts: number, timeElapsedSeconds: number): number;
}

export interface IGameApiClient {
  getCardsByLevel(level: number): Observable<ICardItem[]>;
  getAllCards(): Observable<ICardItem[]>;
  searchCards(keyword: string): Observable<ICardItem[]>;
  addCard(card: Omit<ICardItem, 'cardId'>): Observable<ICardItem>;
  updateCard(id: number, card: ICardItem): Observable<any>;
  updateCardTitle(id: number, title: string): Observable<any>;
  deleteCard(id: number): Observable<any>;
  getAllSessions(): Observable<IGameSession[]>;
  addSession(session: IGameSession): Observable<IGameSession>;
  deleteSession(id: number): Observable<any>;
  getPlayerById(id: number): Observable<IPlayer>;
  login(email: string, password: string): Observable<IPlayer>;
  registerPlayer(player: Partial<IPlayer>): Observable<IPlayer>;
}

export interface IAuthStateService {
  getPlayer(): IPlayer | null;
  setPlayer(player: IPlayer | null): void;
  getAdmin(): IPlayer | null;
  setAdmin(admin: IPlayer | null): void;
  logoutPlayer(): void;
  logoutAdmin(): void;
}
