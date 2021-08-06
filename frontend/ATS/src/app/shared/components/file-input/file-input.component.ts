import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';

@Component({
  selector: 'app-file-input',
  templateUrl: './file-input.component.html',
  styleUrls: ['./file-input.component.scss'],
})
export class FileInputComponent implements OnInit {
  @Input() public image: boolean = false;
  @Input() public accept?: string;
  @Input() public multiple: boolean = false;
  @Input() public default?: File | File[];

  @Output() public upload: EventEmitter<File> = new EventEmitter<File>();

  @Output() public uploadMultiple: EventEmitter<File[]> = new EventEmitter<
  File[]
  >();

  @ViewChild('realInput') public realInput!: ElementRef<HTMLInputElement>;

  public iconName!: string;
  public dragObjectName!: string;
  public inputAccept!: string;
  public dragging: boolean = false;
  public chosen: File[] = [];

  public ngOnInit(): void {
    this.initVariables();
  }

  public focusRealInput(): void {
    this.realInput.nativeElement.focus();
  }

  public removeFile(index: number): void {
    const newChosen = [...this.chosen];
    newChosen.splice(index, 1);
    this.chosen = [...newChosen];
  }

  public cancelEvent(event: Event): void {
    event.preventDefault();
    event.stopPropagation();
  }

  public setDragging(event: Event): void {
    this.cancelEvent(event);
    this.dragging = true;
  }

  public setNotDragging(event: Event): void {
    this.cancelEvent(event);
    this.dragging = false;
  }

  public setFiles(event: DragEvent): void {
    this.setNotDragging(event);

    const transfered: File[] = event.dataTransfer?.files
      ? Array.from(event.dataTransfer?.files)
      : [];

    if (this.multiple) {
      this.chosen = [...this.chosen, ...transfered];
    } else {
      this.chosen = [...(transfered.length > 0 ? [transfered[0]] : [])];
    }
  }

  private initVariables(): void {
    this.iconName = this.image ? 'image' : 'file';

    if (this.accept) {
      this.inputAccept = this.accept;
    } else {
      this.inputAccept = this.image ? 'image/*' : '*/*';
    }

    if (this.multiple) {
      this.dragObjectName = this.image ? 'images' : 'files';
    } else {
      this.dragObjectName = this.image ? 'image' : 'file';
    }

    if (this.default) {
      if (Array.isArray(this.default)) {
        if (this.multiple) {
          this.chosen = [...this.default];
        } else if (this.default.length > 0) {
          this.chosen = [this.default[0]];
        }
      } else {
        this.chosen = [this.default];
      }
    }
  }
}
