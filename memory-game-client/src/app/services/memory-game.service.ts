import { Injectable } from '@angular/core';
import { IBoardCard, ICardItem, IGameEngineService } from '../models/game.interfaces';

@Injectable({
  providedIn: 'root'
})
export class MemoryGameService implements IGameEngineService {

  buildBoardDeck(cards: ICardItem[], level: number): IBoardCard[] {
    const requiredPairs = level === 1 ? 6 : 10;
    const selectedCards = cards.slice(0, requiredPairs);

    const pairedDeck: IBoardCard[] = [];
    let uniqueIdCounter = 1;

    for (const item of selectedCards) {
      pairedDeck.push({
        uniqueBoardId: uniqueIdCounter++,
        cardId: item.cardId,
        title: item.title,
        icon: item.icon,
        category: item.category,
        accentColor: item.accentColor,
        isFlipped: false,
        isMatched: false
      });

      pairedDeck.push({
        uniqueBoardId: uniqueIdCounter++,
        cardId: item.cardId,
        title: item.title,
        icon: item.icon,
        category: item.category,
        accentColor: item.accentColor,
        isFlipped: false,
        isMatched: false
      });
    }

    for (let i = pairedDeck.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [pairedDeck[i], pairedDeck[j]] = [pairedDeck[j], pairedDeck[i]];
    }

    return pairedDeck;
  }

  areCardsMatching(firstCard: IBoardCard, secondCard: IBoardCard): boolean {
    return (
      firstCard.uniqueBoardId !== secondCard.uniqueBoardId &&
      firstCard.cardId === secondCard.cardId
    );
  }

  calculateScore(level: number, attempts: number, timeElapsedSeconds: number): number {
    const basePoints = level === 1 ? 1000 : 1800;
    const minOptimalAttempts = level === 1 ? 6 : 10;
    const extraAttempts = Math.max(0, attempts - minOptimalAttempts);

    const attemptPenalty = extraAttempts * 35;
    const timePenalty = Math.floor(timeElapsedSeconds * 4);

    return Math.max(150, basePoints - attemptPenalty - timePenalty);
  }
}
