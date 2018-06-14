import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
//import { Home } from './components/Home';
import { App } from './components/App';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';

export const routes = <Layout>
    <Route exact path='/' component={ App } />
    <Route path='/counter' component={ Counter } />
    <Route path='/fetchdata' component={ FetchData } />
</Layout>;
