import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InfiniteGuideListComponent } from './infinite-guide-list.component';

describe('InfiniteGuideListComponent', () => {
  let component: InfiniteGuideListComponent;
  let fixture: ComponentFixture<InfiniteGuideListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InfiniteGuideListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InfiniteGuideListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
