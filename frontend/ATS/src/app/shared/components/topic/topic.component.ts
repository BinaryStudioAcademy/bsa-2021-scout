import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-topic',
  templateUrl: './topic.component.html',
  styleUrls: ['./topic.component.scss'],
})
export class TopicComponent {
  @Input() public removable: boolean = false;
  @Input() public text!: string;
  @Output() public remove: EventEmitter<void> = new EventEmitter<void>();

  public emitRemove(): void {
    this.remove.emit();
  }
}
