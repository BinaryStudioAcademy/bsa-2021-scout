import { NativeDateAdapter } from '@angular/material/core';
import { TimezonePipe } from '../pipes/timezone-pipe';

interface DisplayFormat {
  year?: string;
  month?: string;
  day?: string;  
}

export class TimezoneDateAdapter extends NativeDateAdapter {
  public override format(
    date: Date,
    displayFormat: DisplayFormat,
  ): string {
    if (Object.values(displayFormat).every(val => val === 'numeric')) {
      return new TimezonePipe().transform(date);
    }

    return super.format(date, displayFormat);
  }
}
