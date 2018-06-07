import * as React from "react";
import "./App.css";

import { ChangeEvent, MouseEvent } from "react";

class App extends React.Component {
  constructor(props: any) {
    super(props);
    this.handleFileSelected = this.handleFileSelected.bind(this);
    this.handleClick = this.handleClick.bind(this);
  }
  public handleFileSelected(e: ChangeEvent<HTMLInputElement>) {
    // tslint:disable-next-line:no-console
    console.log(e.currentTarget.value);
    // tslint:disable-next-line:no-console
    console.log(e.currentTarget.files);
    if (e.currentTarget.files != null) {
      const f = e.currentTarget.files[0];
      const reader = new FileReader();
      const name = f.name;
      // tslint:disable-next-line:no-console
      console.log("File name: " + name);

      reader.onload = (fr: FileReaderProgressEvent) => {
        const target: any = fr.target;
        const data = target.result;

        // tslint:disable-next-line:no-console
        console.log(data);

        fetch("http://localhost:55856/api/YnabImporter", {
          body: JSON.stringify(data),
          headers: {
            "Content-Type": "application/json-patch+json"
          },
          method: "POST"
        }).then(response => {
          // tslint:disable-next-line:no-console
          console.log(response);
          // tslint:disable-next-line:no-console
          console.log(response.json());
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

export default App;
