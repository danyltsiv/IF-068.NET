import { Component, Injectable, Input, OnInit, OnDestroy } from '@angular/core';

@Injectable()
export class ShowError {
    errorViews: ErrorView[] = [];

    showError(error: string) {

        console.error(error);

        for (var errorView of this.errorViews)
            errorView.setError(error);
    }

    addErrorView(errorView: ErrorView): void {
        if (!this.errorViews.includes(errorView))
            this.errorViews.push(errorView);
    }

    deleteErrorView(errorView: ErrorView): void {
        if (this.errorViews.includes(errorView))
            this.errorViews.slice(this.errorViews.indexOf(errorView), 1);
    }
}

@Component({
    moduleId: module.id,
    selector: 'error-view',
    templateUrl: 'show-error.component.html'
})
export class ErrorView implements OnInit, OnDestroy {
    errorText: string = null;
    timeout = 7000;
    timer: NodeJS.Timer;

    constructor(
        private showError: ShowError
    ) { }

    setError(errorText: string): void{
        this.errorText = errorText;
        if (this.timer != null)
            clearTimeout(this.timer);
        this.timer = setTimeout(() => {
            this.errorText = null, this.timeout;
            this.timer = null;
        }, this.timeout);
    }

    ngOnInit(): void {
        this.showError.addErrorView(this);
    }

    ngOnDestroy(): void {
        this.showError.deleteErrorView(this);
    }

    closeErrorView(): void {
        this.errorText = null;
        if (this.timer != null)
            clearTimeout(this.timer);
    }
}