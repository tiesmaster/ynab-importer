import * as React from "react";
import { RouteComponentProps } from "react-router";
import * as FileDownload from "js-file-download";

import { ChangeEvent, MouseEvent } from "react";

export class App extends React.Component<RouteComponentProps<{}>, {}> {
  constructor(props: any) {
    super(props);
    this.handleFileSelected = this.handleFileSelected.bind(this);
  }
  public handleFileSelected(e: ChangeEvent<HTMLInputElement>) {
    e.preventDefault();

    if (e.currentTarget.files == null) {
      return;
    }

    this.processFile(e.currentTarget.files[0]);
  }
  private async processFile(inputCsvFile: File) {
    const inputCsvText = await this.readFile(inputCsvFile);
    const ynabCsvText = await this.convertToYnab(inputCsvText);
    FileDownload(ynabCsvText, "ynab.csv", "text/csv");
  }
  private readFile(fileHandle: File) {
    return new Promise<string>((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = async (e: ProgressEvent) => {
        const target: any = e.target;
        const fileContent: string = target.result;
        resolve(fileContent);
      };
      reader.readAsText(fileHandle);
    });
  }
  private async convertToYnab(raboCsvText: string) {
    const response = await fetch("/api/YnabImporter", {
      body: JSON.stringify(raboCsvText),
      headers: {
        "Content-Type": "application/json-patch+json"
      },
      method: "POST"
    });
    return await response.text();
  }

  public render() {
    return (
      <div className="App">
        <form>
          <label htmlFor="file">Select Rabobank CSV</label>
          <input type="file" id="file" onChange={this.handleFileSelected} />
        </form>
      </div>
    );
  }
}
