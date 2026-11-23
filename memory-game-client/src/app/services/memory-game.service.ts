import { Injectable } from '@angular/core';
import { IBoardCard, ICardItem, IGameEngineService } from '../models/game.interfaces';

@Injectable({
  providedIn: 'root'
})
export class MemoryGameService implements IGameEngineService {

  buildBoardDeck(cards: ICardItem[], _level: number): IBoardCard[] {
    const pairedDeck: IBoardCard[] = [];
    let uniqueIdCounter = 1;

    for (const item of cards) {
      const cardPoints = item.points && item.points > 0 ? item.points : 100;
      pairedDeck.push({
        uniqueBoardId: uniqueIdCounter++,
        cardId: item.cardId,
        title: item.title,
        icon: item.icon,
        category: item.category,
        accentColor: item.accentColor,
        points: cardPoints,
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
        points: cardPoints,
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

  calculateScore(
    _level: number,
    _attempts: number,
    _timeElapsedSeconds: number,
    matchedCardPoints: number = 0
  ): number {
    return Math.max(0, matchedCardPoints);
  }
}
