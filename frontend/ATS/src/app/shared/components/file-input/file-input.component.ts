import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';

import { FileType } from '../../enums/file-type.enum';

@Component({
  selector: 'app-file-input',
  templateUrl: './file-input.component.html',
  styleUrls: ['./file-input.component.scss'],
})
export class FileInputComponent implements OnInit {
  @Input() public image: boolean = false;
  @Input() public accept?: FileType;
  @Input() public single: boolean = false;
  @Input() public default?: string[];

  @Output() public upload: EventEmitter<File[]> = new EventEmitter<File[]>();

  @Output() public removeDefault: EventEmitter<string[]> = new EventEmitter<
  string[]
  >();

  @ViewChild('realInput') public realInput!: ElementRef<HTMLInputElement>;

  public iconName!: string;
  public dragObjectName!: string;
  public inputAccept!: string;
  public dragging: boolean = false;
  public chosen: File[] = [];
  public arrayDefault: string[] = [];

  public ngOnInit(): void {
    this.initVariables();
  }

  public focusRealInput(): void {
    this.realInput.nativeElement.click();
  }

  public removeFile(index: number): void {
    const newChosen: File[] = [...this.chosen];
    newChosen.splice(index, 1);
    this.chosen = [...newChosen];
    this.upload.emit(this.chosen);
  }

  public emitRemoveDefault(index: number): void {
    const newDefault: string[] = [...this.arrayDefault];
    newDefault.splice(index, 1);
    this.arrayDefault = [...newDefault];
    this.removeDefault.emit([...newDefault]);
  }

  public clear(): void {
    this.chosen = [];
    this.arrayDefault = [];

    this.upload.emit([]);
    this.removeDefault.emit([]);
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
      ? Array.from(event.dataTransfer.files)
      : [];

    if (this.single && transfered.length) {
      this.chosen = [...transfered];
    } else {
      this.chosen = [...this.chosen, ...transfered];
    }

    this.limitFiles();
    this.realInput.nativeElement.value = ''; // Reset value
    this.upload.emit(this.chosen);
  }

  public onChangeRealInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    const files = Array.from(input.files ?? []);

    if (this.single && files.length) {
      this.chosen = [...files];
    } else {
      this.chosen = [...this.chosen, ...files];
    }

    this.limitFiles();
    this.realInput.nativeElement.value = ''; // Reset value
    this.upload.emit(this.chosen);
  }

  private initVariables(): void {
    this.iconName = this.image ? 'image' : 'insert_drive_file';
    this.arrayDefault = [...(this.default ?? [])];
    this.limitDefault();

    if (this.accept) {
      this.inputAccept = this.accept;
    } else {
      this.inputAccept = this.image ? 'image/*' : '*/*';
    }

    if (this.single) {
      this.dragObjectName = this.image ? 'image' : 'file';
    } else {
      this.dragObjectName = this.image ? 'images' : 'files';
    }
  }

  private limitFiles(): void {
    if (this.single) {
      this.chosen = this.chosen.slice(0, 1);
    } else {
      const distinct: File[] = [];
      const detectedNames: string[] = [];

      this.chosen.forEach((file) => {
        if (detectedNames.includes(file.name)) {
          return;
        }

        distinct.push(file);
        detectedNames.push(file.name);
      });

      this.chosen = distinct;
    }
  }

  private limitDefault(): void {
    if (this.single) {
      this.arrayDefault = this.arrayDefault.slice(0, 1);
    }
  }
}
