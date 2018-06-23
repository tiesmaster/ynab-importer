import * as React from "react";
import { Route } from "react-router-dom";
import { App } from "./components/App";

export const routes = (
    <Route exact path="/" component={App} />
);
