import * as Raven from 'raven-js';

import {BrowserModule} from '@angular/platform-browser';
import {ErrorHandler, NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';
import {HttpModule} from "@angular/http";


import {AppComponent} from './app.component';
import {NavMenuComponent} from './components/nav-menu/nav-menu';
import {HomeComponent} from './home/home.component';
import {CounterComponent} from './counter/counter.component';
import {FetchDataComponent} from './fetch-data/fetch-data.component';
import {VehicleFormComponent} from './components/vehicle-form/vehicle-form';

import {VehicleService} from "./services/vehicle.service";
import {ToastyModule} from "ng2-toasty";
import {AppErrorHandler} from "./app.error-handler";
import { VehicleListComponent } from './components/vehicle-list/vehicle-list.component';
import {PaginationComponent} from "./components/shared/pagination.component";
import { ViewVehicleComponent } from './components/view-vehicle/view-vehicle.component';


Raven
  .config('https://d4eb58f2a8e0467695a066faa6cc2779@sentry.io/1245276')
  .install();



@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    VehicleFormComponent,
    VehicleListComponent,
    PaginationComponent,
    ViewVehicleComponent

  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    HttpModule,
    FormsModule,
    ToastyModule.forRoot(),
    RouterModule.forRoot([
      { path: '', redirectTo: 'vehicles', pathMatch: 'full' },
      { path: 'vehicles/new', component: VehicleFormComponent },
      { path: 'vehicles/edit/:id', component: VehicleFormComponent },
      { path: 'vehicles/:id', component: ViewVehicleComponent },
      { path: 'vehicles', component: VehicleListComponent },
      { path: 'home', component: HomeComponent },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: '**', redirectTo: 'home' }
    ])
  ],
  providers: [
    VehicleService,
    {provide: ErrorHandler, useClass: AppErrorHandler}

  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
