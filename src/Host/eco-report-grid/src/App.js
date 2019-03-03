import axios from "axios";
import React, { Component, Fragment } from "react";
import PropTypes from "prop-types";
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
  Button,
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Spinner
} from "reactstrap";
import moment from "moment";
import { ApiClient } from "./ApiClient";
import qs from "qs";

/**
  @typedef {Object} ActivityPerformance
  @property {string} perform
  @property {string} activityName
 */

/**
  @typedef {Object} DailyActivityPerform
  @property {number} stationNo
  @property {string} locationName
  @property {number} locationId
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
  @typedef {Object} DailyReport
  @property {number} pkActivityPerformDetailId
  @property {number} fkActivityPerformId
  @property {number} fkActivityId
  @property {Date} createdOn
  @property {string} perform
  @property {Boolean} isPerform
  @property {string} name
*/

/**
  @typedef {Object} State
  @property {Report[]} reports
  @property { Date } fromDate
  @property { Date } toDate
  @property { string } focusedInput
  @property { Number } locationId
  @property { Number } branchId
  @property {Location[]} locations
  @property {DailyReport[]} formData
  @property {Boolean} modal
  @property {number} selectedLocationId
  @property {Boolean} isLoading
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
 * @param {function} toggle
 */
function createTable(report, index, toggle) {
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
        <td>
          <Button
            type="Button"
            onClick={toggle}
            value={dailyActivityPerform.locationId}
          >
            Edit
          </Button>
        </td>
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
              <td rowSpan="1" colSpan={report.activities.length + 1}>
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
            <td>Action</td>
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
    const parsed = qs.parse(window.location.search.substring(1));
    /** @type {State} */
    this.state = {
      reports: [],
      fromDate: "",
      toDate: "",
      locationId: 0,
      branchId: parsed.branchId,
      locations: [],
      modal: false,
      formData: [],
      selectedLocationId: 0,
      isLoading: true
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.toggle = this.toggle.bind(this);
    this.submitForm = this.submitForm.bind(this);
    this.handleFormChange = this.handleFormChange.bind(this);
    this.loadData = this.loadData.bind(this);
  }

  componentDidMount() {
    this.loadData();
  }

  loadData() {
    axios
      .all([
        ApiClient.get(`/Company/data?branchId=${this.state.branchId}`),
        ApiClient.get(`/Company/locations/branchId/${this.state.branchId}`)
      ])
      .then(
        axios.spread((reports, locations) => {
          this.setState({
            reports: [...reports.data],
            locations: [...locations.data],
            isLoading: false
          });
        })
      );
  }

  /**
   * @param {Event} e
   */
  toggle(e) {
    var newState = {
      modal: !this.state.modal
    };
    if (!this.state.modal) {
      newState.selectedLocationId = e.target.value;
    }
    this.setState(
      {
        ...newState
      },
      () => {
        if (this.state.modal) {
          this.loadEditForm(this.state.selectedLocationId, this.state.branchId);
        } else {
          this.setState({
            isLoading: true
          });
          this.loadData();
        }
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
  handleFormChange(event) {
    var name = event.target.name;
    var formData = [...this.state.formData];
    var index = formData.findIndex(function(el) {
      return (el.pkActivityPerformDetailId = name.match(/\d+/)[0]);
    });
    if (name.replace(/[0-9]/g, "") === "performText") {
      formData[index].perform = event.target.value;
    } else if (name.replace(/[0-9]/g, "") === "isPerform") {
      formData[index].isPerform = event.target.checked;
    }
    this.setState({ formData });
    event.preventDefault();
  }

  /**
   * @param {Event} event
   */
  handleSubmit(event) {
    const data = qs.stringify({
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

  loadEditForm(locationId, branchId) {
    ApiClient.get(
      "/Company/GetData?" +
        qs.stringify({
          locationId,
          branchId
        })
    ).then(json => {
      this.setState({
        formData: [...json.data]
      });
    });
  }

  submitForm() {
    ApiClient.post("/Company/UpdateData", [...this.state.formData]).then(() => {
      this.setState(prevState => ({
        modal: !prevState.modal
      }));
    });
  }

  render() {
    const tables = this.state.reports.map((report, index) =>
      createTable(report, index, this.toggle)
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
          {this.state.isLoading ? (
            <div className="d-flex justify-content-center">
              <Spinner color="dark">
                <span class="sr-only">Loading...</span>
              </Spinner>
            </div>
          ) : (
            <div>{tables}</div>
          )}
        </Row>
        <Row>
          <Modal
            isOpen={this.state.modal}
            toggle={this.toggle}
            className={this.props.className}
          >
            <ModalHeader toggle={this.toggle}>Edit</ModalHeader>
            <ModalBody>
              <Table responsive={true} striped={true}>
                <thead>
                  <tr>
                    <th>Activity</th>
                    <th>Date</th>
                    <th>Is Performed</th>
                    <th>Perform</th>
                  </tr>
                </thead>
                <tbody>
                  {this.state.formData.map((data, index) => {
                    var date = new Date(data.createdOn);
                    return (
                      <tr key={index}>
                        <td>{data.name}</td>
                        <td>
                          {date.getDate()}/{date.getMonth()}/
                          {date.getUTCFullYear()}
                        </td>
                        <td className="text-center">
                          <Input
                            type="checkbox"
                            checked={data.isPerform}
                            name={"isPerform" + data.pkActivityPerformDetailId}
                            onChange={this.handleFormChange}
                          />
                        </td>
                        <td>
                          <Input
                            type="number"
                            value={data.perform}
                            name={
                              "performText" + data.pkActivityPerformDetailId
                            }
                            onChange={this.handleFormChange}
                          />
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </Table>
            </ModalBody>
            <ModalFooter>
              <Button color="primary" onClick={this.submitForm}>
                Save
              </Button>{" "}
              <Button color="secondary" onClick={this.toggle}>
                Cancel
              </Button>
            </ModalFooter>
          </Modal>
        </Row>
      </Container>
    );
  }
}

App.propTypes = {
  reports: PropTypes.arrayOf(
    PropTypes.shape({
      stationName: PropTypes.string,
      dailyActivityPerformReport: PropTypes.arrayOf(
        PropTypes.shape({
          stationNo: PropTypes.number,
          locationName: PropTypes.string,
          activityPerform: PropTypes.arrayOf(
            PropTypes.shape({
              perform: PropTypes.string,
              activityName: PropTypes.string
            })
          ),
          locationId: PropTypes.number
        })
      ),
      activities: PropTypes.arrayOf(PropTypes.string)
    })
  ),
  fromDate: PropTypes.instanceOf(Date),
  toDate: PropTypes.instanceOf(Date),
  locationId: PropTypes.number,
  branchId: PropTypes.number,
  locations: PropTypes.arrayOf(
    PropTypes.shape({
      locationId: PropTypes.number,
      locationName: PropTypes.string
    })
  ),
  selectedLocationId: PropTypes.number,
  formData: PropTypes.arrayOf(
    PropTypes.shape({
      pkActivityPerformDetailId: PropTypes.number,
      createdOn: PropTypes.instanceOf(Date),
      perform: PropTypes.string,
      isPerform: PropTypes.bool
    })
  )
};

export default App;
