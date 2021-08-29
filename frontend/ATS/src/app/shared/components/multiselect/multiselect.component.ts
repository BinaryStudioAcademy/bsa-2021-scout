import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';

import { FormControl } from '@angular/forms';
import { MatSelect } from '@angular/material/select';
import _ from 'lodash';

export interface IOption<T = any> {
  id: number | string;
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
  @Input() public placeholder: string = 'Select';
  @Input() public noTopics: boolean = false;
  @Input() public unstyled: boolean = false;

  @Output() public selectedChange: EventEmitter<IOption[]> = new EventEmitter<
  IOption[]
  >();

  @ViewChild('select') public select!: MatSelect;

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

  public compareWith(a: IOption, b: IOption): boolean {
    return a.id === b.id;
  }

  public emitChange(changed: IOption[]): void {
    this.selectedChange.emit(changed);
  }

  public remove(id: number | string): void {
    this.emitChange(this.selected.filter((item) => item.id !== id));
    this.select.disabled = true;

    setTimeout(() => {
      this.select.disabled = false;
    }, 20);
  }
}
