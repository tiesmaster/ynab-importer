import * as React from "react";
import { RouteComponentProps } from 'react-router';
import * as FileDownload from 'js-file-download';

//import "./App.css";

import { ChangeEvent, MouseEvent } from "react";

export class App extends React.Component<RouteComponentProps<{}>, {}> {
    constructor(props: any) {
        super(props);
        this.handleFileSelected = this.handleFileSelected.bind(this);
        this.handleClick = this.handleClick.bind(this);
    }
    public handleFileSelected(e: ChangeEvent<HTMLInputElement>) {
        if (e.currentTarget.files != null) {
            const f = e.currentTarget.files[0];
            const reader = new FileReader();
            const name = f.name;

            reader.onload = (fr: ProgressEvent) => {
                const target: any = fr.target;
                const data = target.result;

                fetch("/api/YnabImporter", {
                    body: JSON.stringify(data),
                    headers: {
                        "Content-Type": "application/json-patch+json"
                    },
                    method: "POST"
                }).then(response => {
                    response.text().then(csv => {
                        console.log(csv);
                        FileDownload(csv, "ynab.csv", "text/csv");
                    });
                });
            };
            reader.readAsText(f);
        }

        e.preventDefault();
    }
    public handleClick(e: MouseEvent<HTMLButtonElement>) {
        // tslint:disable-next-line:no-console
        console.log("Hoi");
        e.preventDefault();
    }
    public render() {
        return (
            <div className="App">
                <form>
                    <label htmlFor="file">Select Rabobank CSV</label>
                    <input type="file" id="file" onChange={this.handleFileSelected} />
                    <button onClick={this.handleClick}>Convert to YNAB CSV</button>
                </form>
            </div>
        );
    }
}

//export default App;