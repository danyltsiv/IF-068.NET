import { platformBrowserDynamic } from "@angular/platform-browser-dynamic";
import { HttpModule } from '@angular/http';
import { AppModule } from "./app_module/app.module";
const platform = platformBrowserDynamic();
platform.bootstrapModule(AppModule);
