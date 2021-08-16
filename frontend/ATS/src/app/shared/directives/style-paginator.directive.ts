import {
  Directive,
  Host,
  Optional,
  Renderer2,
  Self,
  ViewContainerRef,
  Input,
  EventEmitter,
  AfterViewInit,
} from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';

@Directive({
  selector: '[appStylePaginator]',
})
export class StylePaginatorDirective implements AfterViewInit{
  private _currentPage: number = 0;
  private _pageGapTxt: string = '...';
  private _pageGapTxtIndex: number = -1;
  private _rangeStart: number = 0;
  private _rangeEnd: number = 0;
  private _buttons: Array<HTMLElement> = [];
  public applyFilter$: EventEmitter<void> = new EventEmitter();


  @Input()
  get showTotalPages(): number { return this._showTotalPages; }
  set showTotalPages(value: number) {
    this._showTotalPages = value % 2 == 0 ? value + 1 : value;
  }
  private _showTotalPages = 2;

  constructor(
    @Host() @Self() @Optional()
    private readonly matPag: MatPaginator,
    private vr: ViewContainerRef,
    private ren: Renderer2) { }

  private buildPageNumbers() {
    const actionContainer: HTMLElement = this.vr.element.nativeElement.querySelector(
      'div.mat-paginator-range-actions',
    );
    const nextPageNode: HTMLElement = this.vr.element.nativeElement.querySelector(
      'button.mat-paginator-navigation-next',
    );

    if (this._buttons.length > 0) {
      this._buttons.forEach(button => {
        this.ren.removeChild(actionContainer, button);
      });
      this._buttons.length = 0;
    }

    let dots: boolean = false;

    for (let i = 0; i < this.matPag.getNumberOfPages(); i = i + 1) {
      if (i >= this._rangeStart && i <= this._rangeEnd) {
        this.ren.insertBefore(
          actionContainer,
          this.createButton(i, this.matPag.pageIndex),
          nextPageNode,
        );
      }

      if (i > this._rangeEnd && !dots) {
        this.ren.insertBefore(
          actionContainer,
          this.createButton(this._pageGapTxtIndex, this.matPag.pageIndex),
          nextPageNode,
        );
        dots = true;
      }
    }
  }

  private createButton(i: number, pageIndex: number): any {
    const linkBtn = this.ren.createElement('p');

    const pagingTxt: string = i == this._pageGapTxtIndex ? this._pageGapTxt : (i + 1).toString();
    const text = this.ren.createText(pagingTxt);

    this.ren.addClass(linkBtn, 'mat-custom-page');

    switch (i) {
      case pageIndex:
        this.ren.setAttribute(linkBtn, 'disabled', 'disabled');
        this.ren.addClass(linkBtn, 'selected');
        break;
      case this._pageGapTxtIndex:
        this.ren.listen(linkBtn, 'click', () => {
          if (this._currentPage + this._showTotalPages >= this.matPag.getNumberOfPages()) {
            this.switchPage(this.matPag.getNumberOfPages() - 1);
          } else {
            this.switchPage(this._currentPage + this._showTotalPages);
          }
        });
        break;
      default:
        this.ren.listen(linkBtn, 'click', () => {
          this.switchPage(i);
        });
        break;
    }

    this.ren.appendChild(linkBtn, text);
    this._buttons.push(linkBtn);

    return linkBtn;
  }

  private initPageRange(): void {
    this._rangeStart = this._currentPage - this._showTotalPages / 2;
    this._rangeEnd = this._currentPage + this._showTotalPages / 2;

    if (this._rangeEnd > this.matPag.getNumberOfPages()) {
      this._rangeStart = this.matPag.getNumberOfPages() - this.showTotalPages;
      this._rangeEnd = this.matPag.getNumberOfPages();
    }

    if (this._rangeStart < 0) {
      this._rangeStart = 0;
      this._rangeEnd = this.showTotalPages - 1;
    }

    this.buildPageNumbers();
  }

  private switchPage(i: number): void {
    this._currentPage = i;
    this.matPag.pageIndex = i;
    this.matPag.page.emit();
    this.initPageRange();
  }

  public ngAfterViewInit() {
    setTimeout(() => {
      this.initPageRange();
    }, 0);

    this.applyFilter$.subscribe(_ => {
      setTimeout(() => {
        this.initPageRange();
      }, 0);
    });


    this.matPag.page.subscribe(_ => {
      this._currentPage = this.matPag.pageIndex;
      this.initPageRange();
    });
    
  }
}
