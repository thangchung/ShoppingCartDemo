import React, { Component } from "react";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import { Link, browserHistory } from "react-router";
import * as checkoutActions from "../../redux/modules/checkout";
import {
  Button,
  Card,
  CardText,
  CardBlock,
  CardTitle,
  CardSubtitle,
  Container,
  Row,
  Col,
  Table,
  Form,
  FormGroup,
  Input
} from "reactstrap";
import ShoppingCart from "../../components/Home/ShoppingCart";

class Checkout extends Component {
  componentDidMount() {
    this.props.fetchDefaultData();
  }

  doCheckout(products, shipInfo) {
    this.props.doCheckout(products, shipInfo).then();
    browserHistory.push("/");
  }

  render() {
    const { cart } = this.props.homeStore;
    const { shipInfo } = this.props.checkoutStore;
    return (
      <Card>
        <CardBlock>
          <CardTitle>Checkout Confirmation</CardTitle>
          <ShoppingCart products={cart} isConfirmPage="true" />
          <Form>
            <FormGroup>
              <div>Address: {shipInfo.address}</div>
            </FormGroup>
            <FormGroup>
              <div>City: {shipInfo.city}</div>
            </FormGroup>
            <FormGroup>
              <div>Region: {shipInfo.region}</div>
            </FormGroup>
            <FormGroup>
              <div>PostalCode: {shipInfo.postalCode}</div>
            </FormGroup>
            <FormGroup>
              <div>Country: {shipInfo.country}</div>
            </FormGroup>
          </Form>
          <Link className="btn btn-link" to="/">Back</Link>
          <Button
            color="primary"
            onClick={() => this.doCheckout(cart, shipInfo)}
          >
            Confirm Checkout
          </Button>
        </CardBlock>
      </Card>
    );
  }
}

function mapStateToProps(state, ownProps) {
  return {
    checkoutStore: state.checkoutStore,
    homeStore: state.homeStore
  };
}

export const mapDispatchToProps = dispatch =>
  bindActionCreators(checkoutActions, dispatch);

export default connect(mapStateToProps, mapDispatchToProps)(Checkout);
