import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'timeFormat',
  standalone: true
})
export class TimeFormatPipe implements PipeTransform {
  transform(totalSeconds: number | null | undefined): string {
    if (totalSeconds === null || totalSeconds === undefined || totalSeconds < 0) {
      return '00:00';
    }
    const mins = Math.floor(totalSeconds / 60);
    const secs = totalSeconds % 60;
    const minsStr = mins < 10 ? `0${mins}` : `${mins}`;
    const secsStr = secs < 10 ? `0${secs}` : `${secs}`;
    return `${minsStr}:${secsStr}`;
  }
}

