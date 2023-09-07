import {ComponentFixture, TestBed} from '@angular/core/testing';

import {PartiesComponent} from './parties.component';
import {PartyItemComponent} from "../../components/party-item/party-item.component";
import {MatCardModule} from "@angular/material/card";
import {MatIconModule} from "@angular/material/icon";
import {RouterTestingModule} from "@angular/router/testing";
import {HttpClientTestingModule} from "@angular/common/http/testing";

describe('PartiesComponent', () => {
  let component: PartiesComponent;
  let fixture: ComponentFixture<PartiesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PartiesComponent, PartyItemComponent],
      imports: [
        MatCardModule,
        MatIconModule,
        RouterTestingModule,
        HttpClientTestingModule
      ]
    });
    fixture = TestBed.createComponent(PartiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
