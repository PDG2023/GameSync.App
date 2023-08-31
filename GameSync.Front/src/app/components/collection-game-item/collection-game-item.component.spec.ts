import {ComponentFixture, TestBed} from '@angular/core/testing';

import {CollectionGameItemComponent} from './collection-game-item.component';

describe('CollectionGameItemComponent', () => {
  let component: CollectionGameItemComponent;
  let fixture: ComponentFixture<CollectionGameItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CollectionGameItemComponent]
    });
    fixture = TestBed.createComponent(CollectionGameItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
