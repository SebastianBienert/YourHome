import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OfferDetailsComponent } from './offer-details.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatButtonModule, MatCheckboxModule, MatCardModule, MatMenuModule, MatIconModule, MatGridListModule, MatDividerModule, MatListModule, MatDialogModule } from '@angular/material';
import { OfferService } from '../offer.service';
import { ActivatedRoute, Router, convertToParamMap } from '@angular/router';
import { SlideshowModule } from 'ng-simple-slideshow';
import {of} from 'rxjs';
import { ContentLoaderModule } from '@netbasal/ngx-content-loader';
import { NO_ERRORS_SCHEMA } from '@angular/core'

describe('OfferDetailsComponent', () => {
  let component: OfferDetailsComponent;
  let fixture: ComponentFixture<OfferDetailsComponent>;

  class RouterMock {
    navigate(): void {
    }
  }

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OfferDetailsComponent ],
      providers: [
        OfferService,
        { 
          provide: ActivatedRoute, 
          useValue: { snapshot: { paramMap: convertToParamMap( { 'offerId': '1' } ) } }
        },
        {
          provide: Router,
          useClass: RouterMock
        }
      ],
      imports: [ 
          HttpClientTestingModule,
          MatButtonModule,
          MatCheckboxModule,
          MatIconModule,
          MatCardModule,
          MatMenuModule,
          MatIconModule,
          MatGridListModule,
          MatDividerModule,
          MatListModule,
          MatDialogModule,
          ContentLoaderModule,
          SlideshowModule,
        ],
      schemas: [NO_ERRORS_SCHEMA]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OfferDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
