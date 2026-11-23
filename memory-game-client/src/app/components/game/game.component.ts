import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { IBoardCard, IGameSession, IPlayer } from '../../models/game.interfaces';
import { GameApiService } from '../../services/game-api.service';
import { MemoryGameService } from '../../services/memory-game.service';
import { AuthStateService } from '../../services/auth-state.service';
import { TimeFormatPipe } from '../../pipes/time-format.pipe';

@Component({
  selector: 'app-game',
  standalone: true,
  imports: [CommonModule, RouterLink, TimeFormatPipe],
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit, OnDestroy {
  selectedLevel: number = 1;

  boardCards: IBoardCard[] = [];
  flippedCards: IBoardCard[] = [];
  isBoardLocked: boolean = false;

  attempts: number = 0;
  matchedPairsCount: number = 0;
  matchedCardPoints: number = 0;
  totalPairsCount: number = 6;
  elapsedSeconds: number = 0;
  currentScore: number = 0;
  isGameStarted: boolean = false;
  isGameWon: boolean = false;
  private timerInterval: any = null;

  constructor(
    private apiService: GameApiService,
    private gameEngine: MemoryGameService,
    private authState: AuthStateService
  ) {}

  get loggedInPlayer(): IPlayer | null {
    return this.authState.getPlayer();
  }

  ngOnInit(): void {
    this.startNewGame(1);
  }

  ngOnDestroy(): void {
    this.stopTimer();
  }

  startNewGame(level: number): void {
    this.selectedLevel = level;
    this.totalPairsCount = 0;
    this.attempts = 0;
    this.matchedPairsCount = 0;
    this.matchedCardPoints = 0;
    this.elapsedSeconds = 0;
    this.currentScore = 0;
    this.boardCards = [];
    this.flippedCards = [];
    this.isBoardLocked = false;
    this.isGameWon = false;
    this.isGameStarted = false;
    this.stopTimer();

    this.apiService.getCardsByLevel(level).subscribe(cards => {
      this.boardCards = this.gameEngine.buildBoardDeck(cards, level);
      this.totalPairsCount = this.boardCards.length / 2;
    });
  }

  onCardClick(card: IBoardCard): void {
    if (this.isBoardLocked || card.isFlipped || card.isMatched || this.isGameWon) {
      return;
    }

    if (!this.isGameStarted) {
      this.isGameStarted = true;
      this.startTimer();
    }

    card.isFlipped = true;
    this.flippedCards.push(card);

    if (this.flippedCards.length === 2) {
      this.attempts++;
      this.checkMatch();
    }
  }

  checkMatch(): void {
    const first = this.flippedCards[0];
    const second = this.flippedCards[1];

    if (this.gameEngine.areCardsMatching(first, second)) {
      first.isMatched = true;
      second.isMatched = true;
      this.flippedCards = [];
      this.matchedPairsCount++;
      this.matchedCardPoints += first.points;
      this.currentScore = this.gameEngine.calculateScore(
        this.selectedLevel,
        this.attempts,
        this.elapsedSeconds,
        this.matchedCardPoints
      );

      if (this.matchedPairsCount === this.totalPairsCount) {
        this.stopTimer();
        this.isGameWon = true;
        this.saveWinSession();
      }
    } else {
      this.isBoardLocked = true;
      setTimeout(() => {
        first.isFlipped = false;
        second.isFlipped = false;
        this.flippedCards = [];
        this.isBoardLocked = false;
      }, 750);
    }
  }

  saveWinSession(): void {
    const currentPlayer = this.authState.getPlayer();
    if (!currentPlayer) {
      return;
    }

    const newSession: IGameSession = {
      level: this.selectedLevel,
      pairsCount: this.totalPairsCount,
      attempts: this.attempts,
      timeElapsedSeconds: Math.max(1, this.elapsedSeconds),
      score: this.currentScore,
      isCompleted: true,
      playerId: currentPlayer.playerId
    };

    this.apiService.addSession(newSession).subscribe({
      next: () => {
        this.apiService.getPlayerById(currentPlayer.playerId).subscribe({
          next: updated => {
            this.authState.setPlayer(updated);
          },
          error: err => {
            if (err?.status === 404) {
              this.authState.logoutPlayer();
            }
          }
        });
      },
      error: err => {
        if (err?.status === 404) {
          this.authState.logoutPlayer();
        }
      }
    });
  }

  startTimer(): void {
    this.stopTimer();
    this.timerInterval = setInterval(() => {
      this.elapsedSeconds++;
    }, 1000);
  }

  stopTimer(): void {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
      this.timerInterval = null;
    }
  }
}
