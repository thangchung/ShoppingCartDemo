import { combineReducers } from "redux";
import { routerReducer } from "react-router-redux";
import { reducer as oidcReducer } from "redux-oidc";
import home from "./home";
import checkout from "./checkout";
import order from "./order";
import payment from "./payment";
import audit from "./audit";

const reducers = {
  routing: routerReducer,
  oidc: oidcReducer,
  homeStore: home,
  checkoutStore: checkout,
  orderStore: order,
  paymentStore: payment,
  auditStore: audit
};

export default combineReducers(reducers);
