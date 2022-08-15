import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FileUploadModule } from 'ng2-file-upload';
import { BsDatepickerModule} from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { TimeagoModule } from 'ngx-timeago';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    TabsModule.forRoot(), // Модуль для вкладок
    NgxGalleryModule, // Просмотр фоточек
    NgxSpinnerModule, // Крутяшка для визуализации загрузки
    FileUploadModule, // Загрузчик файлов (Картинок)
    BsDatepickerModule.forRoot(), // Ангуляровский календарик
    PaginationModule.forRoot(), // Пагинация
    ButtonsModule.forRoot(), // Кнопки
    TimeagoModule.forRoot() // TimeAgo
  ],
  exports: [
    BsDropdownModule, 
    ToastrModule,
    TabsModule, // Не забыть добавить модуль в экспорт!
    NgxGalleryModule, // Просмотр фоточек
    NgxSpinnerModule, // Крутяшка для визуализации загрузки
    FileUploadModule, // Загрузчик файлов (Картинок)
    BsDatepickerModule, // Ангуляровский календарик
    PaginationModule, // Пагинация
    ButtonsModule, // Кнопочки
    TimeagoModule // TimeAgo
  ]
})
export class SharedModule { }
