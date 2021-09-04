import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';

@Component({
  selector: 'app-mark-bar',
  templateUrl: './mark-bar.component.html',
  styleUrls: ['./mark-bar.component.scss'],
})
export class MarkBarComponent implements AfterViewInit, OnInit {
  @Input() public mark: number = 0;
  @Input() public maxMark: number = 10;
  
  @ViewChild('container') public container!: ElementRef;

  public percent!: number;
  public currentLeftShift!: number;
  public totalLeftShift!: number;

  public constructor(private readonly cdr: ChangeDetectorRef) {}

  public ngOnInit(): void {
    this.percent = this.mark / this.maxMark * 100;
  }

  public ngAfterViewInit(): void {
    this.currentLeftShift = this.container.nativeElement.clientWidth / 100 * this.percent - 8;
    this.totalLeftShift = this.container.nativeElement.clientWidth - 8;
    this.cdr.detectChanges();
  }
}
