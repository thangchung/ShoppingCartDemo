import { combineReducers } from "redux";
import { routerReducer } from "react-router-redux";
import { reducer as oidcReducer } from "redux-oidc";
import home from "./home";
import payment from "./payment";
import audit from "./audit";

const reducers = {
  routing: routerReducer,
  oidc: oidcReducer,
  homeStore: home,
  paymentStore: payment,
  auditStore: audit
};

export default combineReducers(reducers);
