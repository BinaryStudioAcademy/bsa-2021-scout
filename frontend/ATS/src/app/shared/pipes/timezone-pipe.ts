import { Pipe, PipeTransform } from '@angular/core';
import moment from 'moment';

type Format = string | 'fromnow';

@Pipe({
  name: 'timezone',
})
export class TimezonePipe implements PipeTransform {
  public transform(date: string, format?: Format): string;
  public transform(date: Date, format?: Format): string;
  public transform(
    dateOrString: string | Date,
    format: Format = 'DD.MM.YYYY',
  ): string {
    const date = new Date(dateOrString);
    const offsetMinutes = new Date().getTimezoneOffset(); // Negative number
    const minutes = date.getMinutes();

    date.setMinutes(minutes - offsetMinutes);

    if (format === 'fromnow') {
      return moment(date).fromNow();
    }

    return moment(date).format(format);
  }
}
