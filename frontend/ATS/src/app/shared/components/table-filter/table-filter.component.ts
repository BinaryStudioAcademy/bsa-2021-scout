import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChange,
  SimpleChanges,
} from '@angular/core';

import { MatCheckboxChange } from '@angular/material/checkbox';
import _ from 'lodash';
import moment from 'moment';
import { IOption } from '../multiselect/multiselect.component';

export enum FilterType {
  Text = 1,
  Date = 2,
  Number = 3,
  Boolean = 4,
  Multiple = 5,
}

export interface MultipleSettings {
  options: IOption[];
  valueSelector?: (data: any) => any;
}

export interface NumberSettings {
  integer?: boolean;
  min?: number;
  max?: number;
}

export interface FilterDescriptionItem {
  id: string;
  name: string;
  property?: string;
  type?: FilterType;
  numberSettings?: NumberSettings;
  multipleSettings?: MultipleSettings;
}

export type FilterDescription = FilterDescriptionItem[];

export type FilterValue = string | [Date, Date] | number | boolean | IOption[];

@Component({
  selector: 'app-table-filter',
  templateUrl: './table-filter.component.html',
  styleUrls: ['./table-filter.component.scss'],
})
export class TableFilterComponent implements OnChanges {
  @Input() public description!: FilterDescription;
  @Input() public data: Record<string, any>[] = [];

  @Output() public filteredDataChange: EventEmitter<any[]> = new EventEmitter<
  any[]
  >();

  public filtersSelected: Record<string, boolean> = {};
  public filterValues: Record<string, FilterValue> = {};
  public allKeys: string[] = [];
  public menuOpen: boolean = false;
  public all: boolean = false;
  public descriptionMap: Record<string, FilterDescriptionItem> = {};

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes.data) {
      this.applyFilters(changes.data);
    }

    if (changes.description) {
      this.renewDescription(changes.description);
    }
  }

  public isText(type?: FilterType): boolean {
    return !type || type === FilterType.Text;
  }

  public isDate(type?: FilterType): boolean {
    return type === FilterType.Date;
  }

  public isNumber(type?: FilterType): boolean {
    return type === FilterType.Number;
  }

  public isBoolean(type?: FilterType): boolean {
    return type === FilterType.Boolean;
  }

  public isMultiple(type?: FilterType): boolean {
    return type === FilterType.Multiple;
  }

  public setText(filter: FilterDescriptionItem, event: Event): void {
    const input = this.getInput(event);
    this.filterValues[filter.id] = input.value;
    this.applyFilters();
  }

  public setDate(
    filter: FilterDescriptionItem,
    startInput: HTMLInputElement,
    endInput: HTMLInputElement,
  ): void {
    if (!startInput.value || !endInput.value) {
      return;
    }

    const start = moment(startInput.value, 'l').toDate();
    const end = moment(endInput.value, 'l').toDate();

    this.filterValues[filter.id] = [start, end];
    this.applyFilters();
  }

  public setNumber(filter: FilterDescriptionItem, event: Event): void {
    const input = this.getInput(event);
    const value = Number(input.value);
    const settings = filter.numberSettings;

    let newValue = value;

    if (settings?.integer) {
      newValue = Math.floor(newValue);
    }

    if (settings?.min !== undefined && value < settings.min) {
      newValue = settings.min;
    }

    if (settings?.max !== undefined && value > settings.max) {
      newValue = settings.max;
    }

    input.value = String(newValue);
    this.filterValues[filter.id] = newValue;
    this.applyFilters();
  }

  public setBoolean(
    filter: FilterDescriptionItem,
    value: MatCheckboxChange,
  ): void {
    this.filterValues[filter.id] = value.checked;
    this.applyFilters();
  }

  public setMultiple(filter: FilterDescriptionItem, value: IOption[]): void {
    this.filterValues[filter.id] = value;
    this.applyFilters();
  }

  public castOptions(value: FilterValue): IOption[] {
    return value as IOption[];
  }

  public someActive(): boolean {
    const keys = Object.values(this.filtersSelected);
    return keys.length > 0 && keys.some(_.identity);
  }

  public toggleMenu(): void {
    this.menuOpen = !this.menuOpen;
  }

  public toggleFilter(id: string): void {
    const currentValue = this.filtersSelected[id];
    this.filtersSelected[id] = !currentValue;

    const selectedKeys = Object.keys(this.filtersSelected);

    if (!this.equalArrays(selectedKeys, this.allKeys)) {
      this.all = false;
      this.applyFilters();
      return;
    }

    let allTrue: boolean = true;

    Object.values(this.filtersSelected).forEach((value) => {
      allTrue = allTrue && value;
    });

    this.all = allTrue;
    this.applyFilters();
  }

  public toggleAllFilters(): void {
    if (this.all) {
      this.filtersSelected = {};
    } else {
      this.allKeys.forEach((key) => {
        this.filtersSelected[key] = true;
      });
    }

    this.all = !this.all;
    this.applyFilters();
  }

  private equalArrays(arr1: any[], arr2: any[]): boolean {
    if (arr1.length !== arr2.length) {
      return false;
    }

    for (const item of arr1) {
      if (!arr2.includes(item)) {
        return false;
      }
    }

    return true;
  }

  private getInput(event: Event): HTMLInputElement {
    return event.target as HTMLInputElement;
  }

  private applyFilters(change?: SimpleChange): void {
    const data = change?.currentValue ?? this.data;

    if (!data || !data.length) {
      return;
    }

    let filteredData = [...data];

    Object.entries(this.filtersSelected).forEach(([id, isSelected]) => {
      if (!isSelected) {
        return;
      }

      const filter = this.descriptionMap[id];

      if (this.filterValues[filter.id] === undefined) {
        return;
      }

      filteredData = this.applyFilter(filter, filteredData);
    });

    this.filteredDataChange.emit(filteredData);
  }

  private applyFilter(filter: FilterDescriptionItem, data: any[]): any[] {
    const propName = filter.property ?? filter.id;
    const value = this.filterValues[filter.id];

    switch (filter.type) {
      case FilterType.Number: {
        return data.filter((el) => _.get(el, propName) === (value as number));
      }
      case FilterType.Boolean: {
        return data.filter((el) => _.get(el, propName) === (value as boolean));
      }
      case FilterType.Date: {
        const filterVal = value as [Date, Date];

        return data.filter((el) => {
          const dataVal = new Date(_.get(el, propName));

          return (
            dataVal.getTime() >= filterVal[0].getTime() &&
            dataVal.getTime() <= filterVal[1].getTime()
          );
        });
      }
      case FilterType.Multiple: {
        const filterVal = value as IOption[];

        if (!filterVal.length) {
          return data;
        }

        return data.filter((el) => {
          const dataVal = filter.multipleSettings?.valueSelector
            ? filter.multipleSettings.valueSelector(el)
            : el;

          if (Array.isArray(dataVal)) {
            return filterVal.some((opt) => dataVal.includes(opt.value));
          }

          return filterVal.some((opt) => opt.value === dataVal);
        });
      }
      case FilterType.Text:
      default: {
        const regex = new RegExp(value as string, 'gim');
        return data.filter((el) => regex.test(_.get(el, propName)));
      }
    }
  }

  private renewDescription(change: SimpleChange): void {
    const desc = change.currentValue as FilterDescription;
    this.allKeys = desc.map((f) => f.id);

    this.descriptionMap = Object.assign(
      {},
      ...desc.map((f) => ({ [f.id]: f })),
    );
  }
}
