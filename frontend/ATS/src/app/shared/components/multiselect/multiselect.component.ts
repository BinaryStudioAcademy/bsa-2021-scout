import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';

import { FormControl } from '@angular/forms';
import _ from 'lodash';

export interface IOption<T = any> {
  label: string;
  value: T;
}

@Component({
  selector: 'app-multiselect',
  templateUrl: './multiselect.component.html',
  styleUrls: ['./multiselect.component.scss'],
})
export class MultiselectComponent implements OnChanges, OnInit {
  @Input() public data: IOption[] = [];
  @Input() public selected: IOption[] = [];

  @Output() public selectedChange: EventEmitter<IOption[]> = new EventEmitter<
  IOption[]
  >();

  public control: FormControl = new FormControl();

  public ngOnChanges(changes: SimpleChanges) {
    if (
      changes['selected'] &&
      !_.isEqual(
        changes['selected'].previousValue,
        changes['selected'].currentValue,
      )
    ) {
      this.control.setValue(changes['selected'].currentValue);
    }
  }

  public ngOnInit(): void {
    this.control.setValue(this.selected);

    this.control.valueChanges.subscribe((newValue) =>
      this.emitChange(newValue),
    );
  }

  public emitChange(changed: IOption[]): void {
    this.selectedChange.emit(changed);
  }
}
