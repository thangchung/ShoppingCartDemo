import React, { Component } from "react";
import { Link } from "react-router";
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
  Input
} from "reactstrap";

export default class ShoppingCart extends Component {
  render() {
    const {
      products,
      handleEmptyCartClick,
      handleRemoveProductFromCart,
      isConfirmPage
    } = this.props;
    console.log(this.props);
    return (
      <div>
        <Row>
          <Col xs="9">
            <div>
              <Table striped responsive hover>
                <thead>
                  <tr>
                    <td>Name</td>
                    <td>Model</td>
                    <td>Price</td>
                    <td>Quantity</td>
                    <td>&nbsp;</td>
                  </tr>
                </thead>
                <tbody>
                  {products.map(p => (
                    <tr key={p.product.id}>
                      <td>{p.product.name}</td>
                      <td>{p.product.model}</td>
                      <td>${p.product.price}</td>
                      <td>{p.quantity}</td>
                      <td>
                        {!isConfirmPage &&
                        <Button
                          onClick={() => handleRemoveProductFromCart(p.product)}
                          size="sm"
                          color="warning"
                        >
                          Remove
                        </Button>
                        }
                      </td>
                    </tr>
                  ))}
                  <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td><b>Total</b>:</td>
                    <td>$1234</td>
                  </tr>
                </tbody>
              </Table>
            </div>
          </Col>
          <Col xs="3">
            {!isConfirmPage &&
              <Button color="danger" onClick={() => handleEmptyCartClick()}>
                Empty cart
              </Button>}
            {!isConfirmPage &&
              products.length > 0 &&
              <Button color="info">
                <Link to="/checkout">Checkout</Link>
              </Button>}
          </Col>
        </Row>
      </div>
    );
  }
}
