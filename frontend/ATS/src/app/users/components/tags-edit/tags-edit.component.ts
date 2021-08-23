import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ENTER, SPACE } from '@angular/cdk/keycodes';
import { Tag } from 'src/app/shared/models/tags/tag';
import { MatChipInputEvent } from '@angular/material/chips';

@Component({
  selector: 'app-tags-edit',
  templateUrl: './tags-edit.component.html',
  styleUrls: ['./tags-edit.component.scss'],
})
export class TagsEditComponent {
  @Input() public tags: Tag[] = [];
  @Output() public updateTags = new EventEmitter<Tag[]>();

  readonly separatorKeysCodes = [ ENTER, SPACE ] as const;

  public createTag(event: MatChipInputEvent): void {
    const tagName = event.value.trim();

    if (tagName) {
      this.tags.push({
        id: '',
        tagName: tagName,
      });

      event.chipInput!.clear();
      this.updateTags.emit(this.tags);
    }
  }

  public removeTag(tagName: string): void {
    const tagIndex = this.tags.findIndex(t => t.tagName == tagName);

    if (tagIndex >= 0) {
      this.tags.splice(tagIndex, 1);
      this.updateTags.emit(this.tags);
    }
  }
}
