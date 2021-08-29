import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ENTER, SPACE } from '@angular/cdk/keycodes';
import { MailAttachment } from 'src/app/shared/models/mail-attachment/mail-attachment';

@Component({
  selector: 'app-files-edit',
  templateUrl: './files-edit.component.html',
  styleUrls: ['./files-edit.component.scss'],
})
export class FilesEditComponent {
  @Input() public files: MailAttachment[] = [];
  @Output() public updateFiles = new EventEmitter<MailAttachment[]>();

  readonly separatorKeysCodes = [ ENTER, SPACE ] as const;

  public removeFile(id: string): void {
    const fileIndex = this.files.findIndex(t => t.id == id);

    if (fileIndex >= 0) {
      this.files.splice(fileIndex, 1);
      this.updateFiles.emit(this.files);
    }
  }
}
