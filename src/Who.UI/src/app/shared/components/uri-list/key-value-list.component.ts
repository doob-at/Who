import { Component, Input, Output, EventEmitter, TemplateRef, ViewContainerRef, ViewChild, OnInit, ElementRef } from "@angular/core";
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { IOverlayHandle, DoobOverlayService } from '@doob-ng/cdk-helper';
import { GridBuilder, DefaultContextMenuContext } from '@doob-ng/grid';
import { CellEditingStoppedEvent } from '@ag-grid-community/all-modules';

@Component({
    selector: 'key-value-list',
    templateUrl: './key-value-list.component.html',
    styleUrls: ['./key-value-list.component.scss'],
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: KeyValueListComponent,
        multi: true
    }],
})
export class KeyValueListComponent implements ControlValueAccessor, OnInit {


    _entries: Array<{}> = [];

    @Input()
    set entries(value: Array<{}>) {
        let cls = value || [];
        if (cls.length == 0) {
            cls = [];
        }
        this.SetListEntries(cls);

    }
    get entries() {
        return this._entries;
    }

    private SetListEntries(value: Array<{}>) {
        this._entries = value;
        this.grid?.SetData(this._entries);
    }

    @Input() header?: string;

    private _disabled: boolean = false;
    @Input()
    get disabled() {
        return this._disabled;
    };
    set disabled(value: any) {
        if (value === null || value === undefined || value === false) {
            this._disabled = false
        } else {
            this._disabled = !!value
        }
    }


    private _noScrollbar: boolean = false;
    @Input("no-scrollbar")
    get noScrollbar() {
        return this._noScrollbar;
    };
    set noScrollbar(value: any) {
        if (value === null || value === undefined || value === false) {
            this._noScrollbar = false
        } else {
            this._noScrollbar = !!value
        }
    }

    @Input() keyProperty: string = "Id"
    @Input() valueProperty: string = "Value"

    @Input() checkboxSelection: boolean = false;

    @Output() entriesChange: EventEmitter<Array<{}>> = new EventEmitter<Array<{}>>();

    @ViewChild('itemsContextMenu') itemsContextMenu!: TemplateRef<any>
    // @ViewChild('viewportContextMenu') viewportContextMenu: TemplateRef<any>

    grid!: GridBuilder;

    private contextMenu!: IOverlayHandle;

    constructor(
        public overlay: DoobOverlayService,
        public viewContainerRef: ViewContainerRef,
        private element: ElementRef
    ) {

    }

    ngOnInit() {

        var el = this.element.nativeElement as HTMLElement;
        if (el.attributes.getNamedItem("disabled")) {
            this.disabled = true;
        }

        if (el.attributes.getNamedItem("no-scrollbar")) {
            this.noScrollbar = true;
        }

        const builder = new GridBuilder();

        if (this.checkboxSelection) {
            builder.SetColumns(
                c => c.Default("")
                    .SetMaxWidth(40)
                    .SetMaxWidth(40)
                    .SuppressSizeToFit()
                    .Set(cl => {
                        cl.headerCheckboxSelection = this.checkboxSelection;
                        cl.checkboxSelection = this.checkboxSelection;
                    })
            )
        }

        this.grid = builder.SetColumns(c => c.Default(this.valueProperty).SetHeader(this.header ?? "").Editable())
            .WithRowSelection("multiple")
            .WithFullRowEditType()
            .WithShiftResizeMode()
            .OnDataUpdate(data => this.propagateChange(data))
            .OnCellContextMenu(ev => {
                const selected = ev.api.getSelectedNodes();
                if (selected.length == 0 || !selected.includes(ev.node)) {
                    ev.node.setSelected(true, true)
                }

                let vContext = new DefaultContextMenuContext(ev.api, ev.event as MouseEvent)
                this.contextMenu = this.overlay.OpenContextMenu(ev.event as MouseEvent, this.itemsContextMenu, this.viewContainerRef, vContext)
            })
            .OnViewPortContextMenu((ev, api) => {
                api.deselectAll();
                let vContext = new DefaultContextMenuContext(api, ev as MouseEvent)
                this.contextMenu = this.overlay.OpenContextMenu(ev, this.itemsContextMenu, this.viewContainerRef, vContext)
            })
            .OnRowDoubleClicked(el => {
                //console.log("double Clicked", el)

            })
            .SetData(this.entries)
            .StopEditingWhenCellsLoseFocus()
            .SetDataImmutable(d => d[this.keyProperty])
            .OnGridSizeChange(ev => ev.api.sizeColumnsToFit())
            .OnViewPortClick((ev, api) => {
                api.deselectAll();
            })
            .SetGridOptions({
                onCellEditingStopped: (options: CellEditingStoppedEvent) => {
                    let items: Array<any> = [];
                    options.api.forEachNode(function (node) {
                        items.push(node.data);
                    });


                    this.propagateChange(items)
                }
            });
    }


    AddEntry(): void {
        this.entries = [
            ...this.entries,
            {}
        ];
        this.contextMenu.Close();

    }

    RemoveEntry(arr: Array<{}>): void {

        this.SetListEntries(this._entries.filter(d => !arr.includes(d)));
        this.contextMenu.Close();
    }



    propagateChange(value: Array<{}>) {

        const arr = value.filter(Boolean);
        this.entriesChange.emit(arr);
        this.registered.forEach(fn => {
            fn(arr);
        });
    }

    writeValue(value: Array<string>): void {
        this.entries = value;
    }

    private registered: Array<any> = [];
    registerOnChange(fn: any): void {
        if (this.registered.indexOf(fn) === -1) {
            this.registered.push(fn);
        }
    }

    onTouched = () => { };
    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

    setDisabledState?(isDisabled: boolean): void {
        this.disabled = isDisabled;
    }

}



