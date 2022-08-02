import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FileUploadModule } from 'ng2-file-upload';
import { BsDatepickerModule} from 'ngx-bootstrap/datepicker'
import { PaginationModule } from 'ngx-bootstrap/pagination';


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
    PaginationModule.forRoot() // Пагинация
  ],
  exports: [
    BsDropdownModule, 
    ToastrModule,
    TabsModule, // Не забыть добавить модуль в экспорт!
    NgxGalleryModule, // Просмотр фоточек
    NgxSpinnerModule, // Крутяшка для визуализации загрузки
    FileUploadModule, // Загрузчик файлов (Картинок)
    BsDatepickerModule, // Ангуляровский календарик
    PaginationModule // Пагинация
  ]
})
export class SharedModule { }
