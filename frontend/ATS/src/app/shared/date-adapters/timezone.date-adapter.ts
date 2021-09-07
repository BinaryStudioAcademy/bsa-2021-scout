import { Injectable } from '@angular/core';
import { NativeDateAdapter } from '@angular/material/core';
import { TimezonePipe } from '../pipes/timezone-pipe';

@Injectable({
  providedIn: 'root',
})
export class TimezoneDateAdapter extends NativeDateAdapter {
  public override format(date: Date, _displayFormat: any): string {
    const pipe = new TimezonePipe();
    return pipe.transform(date);
  }
}
