import React from "react";
import { IndexRoute, Route } from "react-router";
import SimpleLayout from "./containers/App/SimpleLayout";
import FullLayout from "./containers/App/FullLayout";
import Home from "./containers/Home/Home";
import Admin from "./containers/Home/Admin";
import Login from "./containers/Login/Login";
import Callback from "./containers/Login/Callback";
import NotFound from "./containers/NotFound/NotFound";
import Checkout from "./containers/Home/Checkout";
import Order from "./containers/Order/Order";
import Payment from "./containers/Payment/Payment";
import Audit from "./containers/Audit/Audit";

export default store => {
  const requireLogin = (nextState, replace, cb) => {
    function checkAuth() {
      const { oidc: { user } } = store.getState();
      if (!user) {
        // oops, not logged in, so can't be here!
        replace("/login");
      }
      cb();
    }
    checkAuth();
  };

  return (
    <Route path="/" component={SimpleLayout}>
      <IndexRoute component={Home} />
      <Route name="Login" path="login" component={Login} />
      <Route path="callback" component={Callback} />
      <Route path="checkout" component={Checkout} />

      <Route path="admin" onEnter={requireLogin}>
        <IndexRoute component={Admin} />
        <Route name="order" path="order" component={Order} />
        <Route name="payment" path="payment" component={Payment} />
        <Route name="audit" path="audit" component={Audit} />
      </Route>
      <Route path="*" component={NotFound} status={404} />
    </Route>
  );
};
