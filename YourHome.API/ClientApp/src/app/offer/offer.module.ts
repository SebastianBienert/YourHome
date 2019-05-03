import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OfferDetailsComponent } from './offer-details/offer-details.component';
import { OfferService } from './offer.service';
import { MatButtonModule, MatCheckboxModule, MatCardModule, MatMenuModule, MatIconModule, MatGridListModule, MatDividerModule, MatListModule, MatDialogModule } from '@angular/material';
import { SlideshowModule } from 'ng-simple-slideshow';
import { OffersListComponent } from './offers-list/offers-list.component';
import { EmailDialogComponent } from './email-dialog/email-dialog.component';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';


@NgModule({
  declarations: [OfferDetailsComponent, EmailDialogComponent, OffersListComponent],
  imports: [
    MatMenuModule,
    MatButtonModule,
    MatCheckboxModule,
    MatCardModule,
    MatIconModule,
    MatGridListModule,
    MatDividerModule,
    MatListModule,
    CommonModule,
    SlideshowModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    InfiniteScrollModule
  ],
  providers: [OfferService],
  entryComponents: [
    EmailDialogComponent
  ]
})
export class OfferModule { }
