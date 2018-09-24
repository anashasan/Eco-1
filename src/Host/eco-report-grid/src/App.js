import React, { Component, Fragment } from "react";
import {
  Table,
  Container,
  Row,
  Form,
  FormGroup,
  Label,
  Input,
  Card,
  CardHeader,
  CardBody,
  Button
} from "reactstrap";
import queryString from "query-string";
import moment from "moment";
import { ApiClient } from "./ApiClient";

/**
  @typedef {Object} ActivityPerformance
  @property {string} perform
  @property {string} activityName
 */

/**
  @typedef {Object} DailyActivityPerform
  @property {number} stationNo
  @property {string} locationName
  @property {ActivityPerformance[]} activityPerform
 */

/**
  @typedef {Object} Report
  @property {string} stationName
  @property {DailyActivityPerform[]} dailyActivityPerformReport
  @property {string[]} activities
 */

/**
  @typedef {Object} Location
  @property {number} locationId
  @property {string} locationName
 */

/**
  @typedef {Object} State
  @property {Report[]} reports
  @property { moment } fromDate
  @property { moment } toDate
  @property { string } focusedInput
  @property { Number } locationId
  @property { Number } branchId
  @property {Location[]} locations
 */

/**
 * @param {ActivityPerformance[]} activityPerformance
 * @param {string[]} activities
 */
function createActivities(activityPerformance, activities) {
  const activitiesElements = [];
  if (activityPerformance) {
    for (let index = 0; index <= activities.length - 1; index++) {
      const activity = activities[index];
      const activityInfo = activityPerformance.find(
        i => i.activityName === activity
      );
      if (activityInfo) {
        activitiesElements.push(
          <td rowSpan="1" colSpan="1" key={index}>
            {activityInfo.perform}
          </td>
        );
      } else {
        activitiesElements.push(
          <td rowSpan="1" colSpan="1" key={index}>
            &nbsp;
          </td>
        );
      }
    }
  } else {
    for (let index = 0; index <= activities.length - 1; index++) {
      activitiesElements.push(
        <td rowSpan="1" colSpan="1" key={index}>
          &nbsp;
        </td>
      );
    }
  }

  return activitiesElements;
}

/**
 * @param {Report} report
 * @param {Number} index
 */
function createTable(report, index) {
  const trData = report.dailyActivityPerformReport.map(
    (dailyActivityPerform, index) => (
      <tr key={index}>
        <td rowSpan="1" colSpan="1">
          {dailyActivityPerform.stationNo}
        </td>
        <td rowSpan="1" colSpan="1">
          {dailyActivityPerform.locationName}
        </td>
        {createActivities(
          dailyActivityPerform.activityPerform,
          report.activities
        )}
      </tr>
    )
  );

  return (
    <Fragment key={index}>
      <h4>{report.stationName}</h4>
      <Table className="text-center" striped bordered responsive>
        <tbody>
          <tr className="table-secondary">
            <td rowSpan="1" colSpan="2">
              Station Details
            </td>
            {report.activities && (
              <td rowSpan="1" colSpan={report.activities.length}>
                Activity Perform
              </td>
            )}
          </tr>
          <tr>
            <td rowSpan="1" colSpan="1">
              Station No
            </td>
            <td rowSpan="1" colSpan="1">
              Location Name
            </td>
            {report.activities &&
              report.activities.map((activity, index) => (
                <td rowSpan="1" colSpan="1" key={index}>
                  {activity}
                </td>
              ))}
          </tr>
          {trData}
        </tbody>
      </Table>
    </Fragment>
  );
}

class App extends Component {
  constructor() {
    super();
    const parsed = queryString.parse(window.location.search);
    /** @type {State} */
    this.state = {
      reports: [],
      fromDate: "",
      toDate: "",
      locationId: 0,
      branchId: parsed.branchId,
      locations: []
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  componentDidMount() {
    ApiClient.get(`/Company/data?branchId=${this.state.branchId}`).then(
      json => {
        this.setState({
          reports: [...json.data]
        });
      }
    );

    ApiClient.get(`/Company/locations/branchId/${this.state.branchId}`).then(
      json => {
        this.setState({
          locations: [...json.data]
        });
      }
    );
  }

  /**
   * @param {Event} event
   */
  handleChange(event) {
    var name = event.target.name;
    this.setState({ [name]: event.target.value });
  }

  /**
   * @param {Event} event
   */
  handleSubmit(event) {
    const data = queryString.stringify({
      branchId: this.state.branchId,
      locationId: this.state.locationId === 0 ? null : this.state.locationId,
      fromDate: moment(this.state.fromDate).format("DD/MM/YYYY"),
      toDate: moment(this.state.toDate).format("DD/MM/YYYY")
    });
    ApiClient.get(`/Company/data?${data}`).then(json => {
      this.setState({
        reports: [...json.data]
      });
    });
    event.preventDefault();
  }

  render() {
    const tables = this.state.reports.map((report, index) =>
      createTable(report, index)
    );
    return (
      <Container>
        <br />
        <Row>
          <Card>
            <CardHeader>Search Box</CardHeader>
            <CardBody>
              <Form inline onSubmit={this.handleSubmit}>
                <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
                  <Label for="fromDate" className="mr-sm-2">
                    Starting Date
                  </Label>
                  <Input
                    type="date"
                    id="fromDate"
                    name="fromDate"
                    value={this.state.fromDate}
                    onChange={this.handleChange}
                  />
                </FormGroup>
                <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
                  <Label for="toDate" className="mr-sm-2">
                    Ending Date
                  </Label>
                  <Input
                    type="date"
                    id="toDate"
                    name="toDate"
                    value={this.state.toDate}
                    onChange={this.handleChange}
                  />
                </FormGroup>
                <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
                  <Label for="locationDropDown" className="mr-sm-2">
                    Location
                  </Label>
                  <Input
                    type="select"
                    id="locationDropDown"
                    name="locationId"
                    value={this.state.locationId}
                    onChange={this.handleChange}
                  >
                    <option value={null}>Select All</option>
                    {this.state.locations.map((locationData, index) => (
                      <option key={index} value={locationData.locationId}>
                        {locationData.locationName}
                      </option>
                    ))}
                  </Input>
                </FormGroup>
                <Button>Search</Button>
              </Form>
            </CardBody>
          </Card>
        </Row>
        <br />
        <Row>
          <div>{tables}</div>
        </Row>
      </Container>
    );
  }
}

export default App;
